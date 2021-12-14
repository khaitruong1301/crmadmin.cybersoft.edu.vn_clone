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
    public interface IQuyenService : IService<Quyen, QuyenViewModel>
    {
        Task<ResponseEntity> CheckUser(string idface, string email);

    }

    public class QuyenService : ServiceBase<Quyen, QuyenViewModel>, IQuyenService
    {
        private IQuyenRepository _quyenRepository;

        public QuyenService(IQuyenRepository quyenRepository, IMapper mapper)
            : base(quyenRepository, mapper)
        {
            _quyenRepository = quyenRepository;
        }

        public async Task<ResponseEntity> CheckUser(string idface, string email)
        {
            try
            {
                if(idface != null && idface.Trim() != "")
                {
                    NguoiDung nguoiDung = await _quyenRepository.Off_CheckUserFace(idface);
                    if (nguoiDung != null)
                    {
                        return new ResponseEntity(StatusCodeConstants.OK, nguoiDung);

                    }
                }
                if (email != null && email.Trim() != "")
                {
                    NguoiDung nguoiDung2 = await _quyenRepository.Off_CheckUserEmail(email);
                    if (nguoiDung2 != null)
                    {
                        return new ResponseEntity(StatusCodeConstants.OK, nguoiDung2);

                    }
                    dynamic nguoiDung3 = await _quyenRepository.On_CheckUserEmail(email);
                    if (nguoiDung3 != null)
                    {
                        return new ResponseEntity(StatusCodeConstants.OK, nguoiDung3);

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