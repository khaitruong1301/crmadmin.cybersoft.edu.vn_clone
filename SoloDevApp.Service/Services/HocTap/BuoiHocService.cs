using AutoMapper;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.Utilities;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface IBuoiHocService : IService<BuoiHoc, BuoiHocViewModel>
    {
         
    }

    public class BuoiHocService : ServiceBase<BuoiHoc, BuoiHocViewModel>, IBuoiHocService
    {
        IBuoiHocRepository _buoiHocRepository;
        ILopHocRepository _lopHocRepository;
        IRoadMapDetailRepository _roadMapDetailRepository;
        public BuoiHocService(IBuoiHocRepository buoiHocRepository,ILopHocRepository lopHocRepository,IRoadMapDetailRepository roadMapDetailRepository,
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

    }
}