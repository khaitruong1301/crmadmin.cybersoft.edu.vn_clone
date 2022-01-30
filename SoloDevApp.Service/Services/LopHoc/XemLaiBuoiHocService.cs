using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;

namespace SoloDevApp.Service.Services
{
    public interface IXemLaiBuoiHocService : IService<XemLaiBuoiHoc, XemLaiBuoiHocViewModel>
    {
        Task<ResponseEntity> GetTheoLop(int maLop);

    }

    public class XemLaiBuoiHocService : ServiceBase<XemLaiBuoiHoc, XemLaiBuoiHocViewModel>, IXemLaiBuoiHocService
    {
        private IXemLaiBuoiHocRepository _xemLaiBuoiHocRepository;

        public XemLaiBuoiHocService(IXemLaiBuoiHocRepository xemLaiBuoiHocRepository, IMapper mapper)
            : base(xemLaiBuoiHocRepository, mapper)
        {
            _xemLaiBuoiHocRepository = xemLaiBuoiHocRepository;

        }


        public async Task<ResponseEntity> GetTheoLop(int maLop)
        {
            try
            {

                IEnumerable<XemLaiBuoiHoc> dsXemLaiBuoiHoc = await _xemLaiBuoiHocRepository.GetTheoMaLop(maLop);

                return new ResponseEntity(StatusCodeConstants.OK, dsXemLaiBuoiHoc);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }
    }
}