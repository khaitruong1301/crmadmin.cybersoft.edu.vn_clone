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
    public interface ITrackingNguoiDungService : IService<TrackingNguoiDung, RoadMapDetailViewModel>
    {

    }
    public class RoadMapDetailService : ServiceBase<RoadMapDetail, RoadMapDetailViewModel>, IRoadMapDetailService
    {
        IRoadMapDetailRepository _roadMapDetailRepository;
        public RoadMapDetailService(IRoadMapDetailRepository roadMapDetailRepository,
            IMapper mapper)
            : base(roadMapDetailRepository, mapper)
        {
            _roadMapDetailRepository = roadMapDetailRepository;
        }
    }
}
