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
    public interface IBaiHocNewService : IService<BaiHoc_TaiLieu_Link_TracNghiem, BaiHoc_TaiLieu_Link_TracNghiemViewModel>
    {

    }

    public class BaiHocNewService : ServiceBase<BaiHoc_TaiLieu_Link_TracNghiem, BaiHoc_TaiLieu_Link_TracNghiemViewModel>, IBaiHocNewService
    {
        IBaiHoc_TaiLieu_Link_TracNghiemRepository _baiHocNewRepository;
        IBuoiHocRepository _buoiHocRepository;
        ILoaiBaiHocRepository _loaiBaiHocRepository;
        public BaiHocNewService(IBaiHoc_TaiLieu_Link_TracNghiemRepository baiHocNewRepository,
            ILoaiBaiHocRepository loaiBaiHocRepository,
            IBuoiHocRepository buoiHocRepository,
        IMapper mapper)
            : base(baiHocNewRepository, mapper)
        {
            _baiHocNewRepository = baiHocNewRepository;
            _buoiHocRepository = buoiHocRepository;
            _loaiBaiHocRepository = loaiBaiHocRepository;
        }

        public async override Task<ResponseEntity> InsertAsync(BaiHoc_TaiLieu_Link_TracNghiemViewModel modelVm)
        {
            try
            {
                //Kiểm tra xem buổi học có tồn tại hay không
                BuoiHoc buoiHoc = await _buoiHocRepository.GetSingleByIdAsync(modelVm.MaBuoi);

                if (buoiHoc == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);
                }

                //Kiểm tra xem loại bài tập có tồn tại hay không
                LoaiBaiHoc loaiBaiHoc = await _loaiBaiHocRepository.GetSingleByConditionAsync("Id", modelVm.MaLoaiBaiHoc);
                if (loaiBaiHoc == null)
                {
                    return new ResponseEntity(StatusCodeConstants.NOT_FOUND);
                }

                //Kiểm tra xem nếu không truyền BiDanh thì lấy tiêu đề làm BiDanh nếu có thì convert về chuẩn
                if (modelVm.BiDanh == null || modelVm.BiDanh.Trim().Length == 0)
                {
                    modelVm.BiDanh = FuncUtilities.BestLower(modelVm.TieuDe);
                }
                modelVm.BiDanh = FuncUtilities.BestLower(modelVm.BiDanh);


                //Thêm bài học vào
                BaiHoc_TaiLieu_Link_TracNghiem baiHoc = _mapper.Map<BaiHoc_TaiLieu_Link_TracNghiem>(modelVm);

                baiHoc = await _baiHocNewRepository.InsertAsync(baiHoc);

                if (baiHoc == null)
                {
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, modelVm, MessageConstants.INSERT_ERROR);
                }

                //Thêm bài học vào buổi học

                //Lấy danh sách các bài học của buổi học và thêm Id bài học vào list
                List<int> lsBaiHocTrongBuoi = JsonConvert.DeserializeObject<List<int>>(buoiHoc.DanhSachBaiHocTracNghiem);
                lsBaiHocTrongBuoi.Add(baiHoc.Id);

                //Update lại buổi học với dữ liệu mới
                buoiHoc.DanhSachBaiHocTracNghiem = JsonConvert.SerializeObject(lsBaiHocTrongBuoi);

                if ((await _buoiHocRepository.UpdateAsync(modelVm.MaBuoi, buoiHoc)) == null)
                {
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, buoiHoc, MessageConstants.INSERT_ERROR);
                }

                return new ResponseEntity(StatusCodeConstants.OK, baiHoc, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

    }
}