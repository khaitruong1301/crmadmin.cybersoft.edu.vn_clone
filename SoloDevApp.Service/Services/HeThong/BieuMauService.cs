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
    public interface IBieuMauService : IService<BieuMau, BieuMauViewModel>
    {

    }

    public class BieuMauService : ServiceBase<BieuMau, BieuMauViewModel>, IBieuMauService
    {
        private IBieuMauRepository _bieuMauRepository;

        public BieuMauService(IBieuMauRepository bieuMauRepository, IMapper mapper)
            : base(bieuMauRepository, mapper)
        {
            _bieuMauRepository = bieuMauRepository;
        }

      
    }
}