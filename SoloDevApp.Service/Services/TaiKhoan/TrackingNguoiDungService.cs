using AutoMapper;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace SoloDevApp.Service.Services
{
    public interface ITrackingNguoiDungService : IService<TrackingNguoiDung, TrackingNguoiDungViewModel>
    {
        Task<ResponseEntity> getThongBaoNguoiDung();
    }
    public class TrackingNguoiDungService : ServiceBase<TrackingNguoiDung, TrackingNguoiDungViewModel>, ITrackingNguoiDungService
    {
        private readonly ITrackingNguoiDungRepository _trackingNguoiDungRepository;
        public TrackingNguoiDungService(ITrackingNguoiDungRepository trackingNguoiDungRepository,
            IMapper mapper)
            : base(trackingNguoiDungRepository, mapper)
        {
            _trackingNguoiDungRepository = trackingNguoiDungRepository;
        }

        public async Task<ResponseEntity> getThongBaoNguoiDung()
        {
            try
            {
                //Tam set cung maNguoiDung de test
                string maNguoiDung = "d9699b77-f003-42c4-b050-26607079a789";

                IEnumerable<TrackingNguoiDung> lsTrackingNguoiDung = await _trackingNguoiDungRepository.GetMultiByConditionAsync("MaNguoiDung", maNguoiDung);



                if (lsTrackingNguoiDung == null)
                {
                    return null;
                }

              


                return new ResponseEntity(StatusCodeConstants.NOT_FOUND, "Không tìm thấy lịch sử học tập");
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }


        }
    }
}
