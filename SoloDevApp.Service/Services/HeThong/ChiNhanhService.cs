using System;
using System.Threading.Tasks;
using AutoMapper;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;

namespace SoloDevApp.Service.Services
{
    public interface IChiNhanhService : IService<ChiNhanh, ChiNhanhViewModel>
    {
        Task<ResponseEntity> CheckUser(string idface, string email);

    }

    public class ChiNhanhService : ServiceBase<ChiNhanh, ChiNhanhViewModel>, IChiNhanhService
    {
        private IChiNhanhRepository _chiNhanhRepository;

        public ChiNhanhService(IChiNhanhRepository chiNhanhRepository, IMapper mapper)
            : base(chiNhanhRepository, mapper)
        {
            _chiNhanhRepository = chiNhanhRepository;
        }

        public async Task<ResponseEntity> CheckUser(string idface, string email)
        {
            try
            {
                if(idface != null && idface.Trim() != "")
                {
                    NguoiDung nguoiDung = await _chiNhanhRepository.Off_CheckUserFace(idface);
                    if (nguoiDung != null)
                    {
                        return new ResponseEntity(StatusCodeConstants.OK, nguoiDung);

                    }
                }
                  

                return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, "0");

            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

    }
}