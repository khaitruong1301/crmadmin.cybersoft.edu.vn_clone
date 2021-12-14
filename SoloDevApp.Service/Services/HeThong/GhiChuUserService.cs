using AutoMapper;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface IGhiChuUserService : IService<GhiChuUser, GhiChuUserViewModel>
    {
        Task<ResponseEntity> LayTheoMaLop(int maLop);
    }

    public class GhiChuUserService : ServiceBase<GhiChuUser, GhiChuUserViewModel>, IGhiChuUserService
    {
        private IGhiChuUserRepository _ghiChuUserRepository;
        public GhiChuUserService(IGhiChuUserRepository ghiChuUserRepository, IMapper mapper)
            : base(ghiChuUserRepository, mapper)
        {
            _ghiChuUserRepository = ghiChuUserRepository;
        }

        public async Task<ResponseEntity> LayTheoMaLop(int maLop)
        {
            try
            {

                IEnumerable<GhiChuUser> dsGhiChuUser = await _ghiChuUserRepository.GetTheoMaLop(maLop);

                return new ResponseEntity(StatusCodeConstants.OK, dsGhiChuUser);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }
    }
}