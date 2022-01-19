using AutoMapper;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.Utilities;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface IBuoiHocService : IService<BuoiHoc, BuoiHocViewModel>
    {
        Task<ResponseEntity> ThemListBuoiHocTheoMaLop(InputThemListBuoiHocTheoMaLopViewModel modelVm);
    }

    public class BuoiHocService : ServiceBase<BuoiHoc, BuoiHocViewModel>, IBuoiHocService
    {
        IBuoiHocRepository _buoiHocRepository;
        ILopHocRepository _lopHocRepository;
        IRoadMapDetailRepository _roadMapDetailRepository;
        public BuoiHocService(IBuoiHocRepository buoiHocRepository, ILopHocRepository lopHocRepository, IRoadMapDetailRepository roadMapDetailRepository,
            IMapper mapper)
            : base(buoiHocRepository, mapper)
        {
            _buoiHocRepository = buoiHocRepository;
            _lopHocRepository = lopHocRepository;
            _roadMapDetailRepository = roadMapDetailRepository;

        }

        public async override Task<ResponseEntity> InsertAsync(BuoiHocViewModel modelVm)
        {
            try
            {

                //Kiểm tra lớp học có trong hệ thống hay không
                LopHoc lopHoc = await _lopHocRepository.GetSingleByIdAsync(modelVm.MaLop);
                if (lopHoc == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);
                }

                //Kiểm tra road map detail có trong hệ thống hay không
                RoadMapDetail roadMapDetail = await _roadMapDetailRepository.GetSingleByIdAsync(modelVm.MaRoadMapDetail);
                if (roadMapDetail == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);
                }

                //Thêm buổi học vào hệ thống
                BuoiHoc buoiHoc = _mapper.Map<BuoiHoc>(modelVm);

                //Kiểm tra nếu không truyền BiDanh lên thì tạo BiDanh hoặc convert BiDanh về đúng dạng
                if (buoiHoc.BiDanh == null || buoiHoc.BiDanh.Trim().Length == 0)
                {
                    buoiHoc.BiDanh = FuncUtilities.BestLower(buoiHoc.TenBuoiHoc);
                }
                buoiHoc.BiDanh = FuncUtilities.BestLower(buoiHoc.BiDanh);

                buoiHoc = await _buoiHocRepository.InsertAsync(buoiHoc);

                //Thêm thất bại
                if (buoiHoc == null)
                {
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.INSERT_ERROR);
                }

                //Lấy ra danh sách các buổi học của lớp
                LopHoc lopHocHienTai = await _lopHocRepository.GetSingleByIdAsync(buoiHoc.MaLop);

                List<int> lsCacBuoiHoc = JsonConvert.DeserializeObject<List<int>>(lopHocHienTai.DanhSachBuoi);

                lsCacBuoiHoc.Add(buoiHoc.Id);

                String lsCacBuoiHocString = JsonConvert.SerializeObject(lsCacBuoiHoc);

                lopHocHienTai.DanhSachBuoi = lsCacBuoiHocString;

                if ((await _lopHocRepository.UpdateAsync(buoiHoc.MaLop, lopHocHienTai)) == null)
                {
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, buoiHoc, MessageConstants.INSERT_ERROR);
                }


                return new ResponseEntity(StatusCodeConstants.OK, buoiHoc, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> ThemListBuoiHocTheoMaLop(InputThemListBuoiHocTheoMaLopViewModel modelVm)
        {
            try
            {
                //Kiểm tra lớp học có trong hệ thống hay không
                LopHoc lopHoc = await _lopHocRepository.GetSingleByIdAsync(modelVm.MaLop);
                if (lopHoc == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "Lớp học không tồn tại");
                }

                //Kiểm tra road map detail có trong hệ thống hay không

                if (modelVm.MaRoadMapDetail != 0)
                {
                    RoadMapDetail roadMapDetail = await _roadMapDetailRepository.GetSingleByIdAsync(modelVm.MaRoadMapDetail);
                    if (roadMapDetail == null)
                    {
                        return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "Road map không tồn tại");
                    }
                }

                int bienDemThuTuBuoiHoc = 0;
                DateTime _ngayHoc = lopHoc.NgayBatDau;



                List<int> lsMaBuoiHocNew = new List<int>();

                List<int> thoiKhoaBieu = JsonConvert.DeserializeObject<List<int>>(lopHoc.ThoiKhoaBieu);

                //Map list thời khóa biểu về dạng phù hợp
                thoiKhoaBieu = thoiKhoaBieu.ConvertAll(item => ConvertNgayTrongTuan(item));

                do
                {
                    if (thoiKhoaBieu.Contains((int)_ngayHoc.DayOfWeek))
                    {
                        bienDemThuTuBuoiHoc++;
                        BuoiHoc buoiHoc = new BuoiHoc();
                        buoiHoc.STT = bienDemThuTuBuoiHoc;
                        buoiHoc.NgayHoc = _ngayHoc;
                        buoiHoc.MaLop = modelVm.MaLop;
                        buoiHoc.MaRoadMapDetail = modelVm.MaRoadMapDetail;

                        lsMaBuoiHocNew.Add((await _buoiHocRepository.InsertAsync(buoiHoc)).Id);
                    }
                    //Tăng ngày học lên 1 
                    _ngayHoc = _ngayHoc.AddDays(1);
                } while (bienDemThuTuBuoiHoc <= modelVm.SoBuoiHocCuaLop);

                //Lấy ra lớp học và cập nhật danh sách id buổi học vào
                LopHoc lopHocModel = await _lopHocRepository.GetSingleByIdAsync(modelVm.MaLop);

                lopHocModel.DanhSachBuoi = JsonConvert.SerializeObject(lsMaBuoiHocNew);

                await _lopHocRepository.UpdateAsync(lopHocModel.Id, lopHocModel);

                ////Thêm buổi học vào hệ thống
                //BuoiHoc buoiHoc = _mapper.Map<BuoiHoc>(modelVm);

                ////Kiểm tra nếu không truyền BiDanh lên thì tạo BiDanh hoặc convert BiDanh về đúng dạng
                //if (buoiHoc.BiDanh == null || buoiHoc.BiDanh.Trim().Length == 0)
                //{
                //    buoiHoc.BiDanh = FuncUtilities.BestLower(buoiHoc.TenBuoiHoc);
                //}
                //buoiHoc.BiDanh = FuncUtilities.BestLower(buoiHoc.BiDanh);

                //buoiHoc = await _buoiHocRepository.InsertAsync(buoiHoc);

                ////Thêm thất bại
                //if (buoiHoc == null)
                //{
                //    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.INSERT_ERROR);
                //}

                ////Lấy ra danh sách các buổi học của lớp
                //LopHoc lopHocHienTai = await _lopHocRepository.GetSingleByIdAsync(buoiHoc.MaLop);

                //List<int> lsCacBuoiHoc = JsonConvert.DeserializeObject<List<int>>(lopHocHienTai.DanhSachBuoi);

                //lsCacBuoiHoc.Add(buoiHoc.Id);

                //String lsCacBuoiHocString = JsonConvert.SerializeObject(lsCacBuoiHoc);

                //lopHocHienTai.DanhSachBuoi = lsCacBuoiHocString;

                //if ((await _lopHocRepository.UpdateAsync(buoiHoc.MaLop, lopHocHienTai)) == null)
                //{
                //    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, buoiHoc, MessageConstants.INSERT_ERROR);
                //}

                return new ResponseEntity(StatusCodeConstants.OK);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }
        private int ConvertNgayTrongTuan(int input)
        {
            // thu 2
            if (input == 1 || input == 7 || input == 14)
                return 1;
            // thu 3
            if (input == 2 || input == 8 || input == 15)
                return 2;
            // thu 4
            if (input == 3 || input == 9 || input == 16)
                return 3;
            // thu 5
            if (input == 4 || input == 10 || input == 17)
                return 4;
            // thu 6
            if (input == 5 || input == 11 || input == 18)
                return 5;
            // thu 7
            if (input == 6 || input == 12 || input == 19)
                return 6;
            // CN
            if (input == 13 || input == 20)
                return 0;
            return -1;
        }
    }
}