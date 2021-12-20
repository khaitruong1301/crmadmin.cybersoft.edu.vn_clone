using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Helpers;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.Utilities;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace SoloDevApp.Service.Services
{
    public interface IRoadMapService : IService<RoadMap, RoadMapViewModel>
    {
        Task<ResponseEntity> InsertRoadMapAsync(RoadMapViewModel model);
    }
    public class RoadMapService : ServiceBase<RoadMap, RoadMapViewModel>, IRoadMapService
    {
        IRoadMapRepository _roadMapRepository;
        IRoadMapDetailRepository _roadMapDetailRepository;
        public RoadMapService(IRoadMapRepository roadMapRepository,
            IRoadMapDetailRepository roadMapDetailRepository,
            IMapper mapper)
            : base(roadMapRepository, mapper)
        {
            _roadMapRepository = roadMapRepository;
            _roadMapDetailRepository = roadMapDetailRepository;
        }


        public async Task<ResponseEntity> InsertRoadMapAsync(RoadMapViewModel model)
        {
            try
            {
                var result = await InsertAsync(model);
                
                RoadMap roadMapMoiTao = await _roadMapRepository.GetSingleByConditionAsync("BiDanh", model.BiDanh);

                //Tạo mới road map detail cho road map mới
                RoadMapDetail newRoadMapDetail = new RoadMapDetail();

                newRoadMapDetail.TenRoadMapDetail = String.Concat("Chi tiết ",model.TenRoadMap);
                newRoadMapDetail.BiDanh = newRoadMapDetail.TenRoadMapDetail;

                //Thêm road map detail mới vào DB
                await _roadMapDetailRepository.InsertAsync(newRoadMapDetail);


                return new ResponseEntity(StatusCodeConstants.CREATED, model, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

    }

    
}
