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
    public interface IBuoiHocService : IService<BuoiHoc, BuoiHocViewModel>
    {

    }

    public class BuoiHocService : ServiceBase<BuoiHoc, BuoiHocViewModel>, IBuoiHocService
    {
        IBuoiHocRepository _buoiHocRepository;
        public BuoiHocService(IBuoiHocRepository buoiHocRepository,
            IMapper mapper)
            : base(buoiHocRepository, mapper)
        {
            _buoiHocRepository = buoiHocRepository;

        }
    }
}