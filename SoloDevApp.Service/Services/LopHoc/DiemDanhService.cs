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
    public interface IDiemDanhService : IService<DiemDanh, DiemDanhViewModel>
    {
        Task<ResponseEntity> LayTheoMaLop(int maLop);
    }

    public class DiemDanhService : ServiceBase<DiemDanh, DiemDanhViewModel>, IDiemDanhService
    {
        private IDiemDanhRepository _diemDanhRepository;
        public DiemDanhService(IDiemDanhRepository diemDanhRepository, IMapper mapper)
            : base(diemDanhRepository, mapper)
        {
            _diemDanhRepository = diemDanhRepository;
        }

        public async Task<ResponseEntity> LayTheoMaLop(int maLop)
        {
            try
            {
              
                IEnumerable<DiemDanh> dsDiemDanh = await _diemDanhRepository.GetTheoMaLop(maLop);



                return new ResponseEntity(StatusCodeConstants.OK, dsDiemDanh);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }
    }
}