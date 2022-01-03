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


        public override async Task<ResponseEntity> InsertAsync(RoadMapViewModel modelVm)
        {
            try
            {
                
                
                RoadMap entity = _mapper.Map<RoadMap>(modelVm);
                if (entity.BiDanh == null || entity.BiDanh.Trim().Length == 0)
                {
                    entity.BiDanh = FuncUtilities.BestLower(entity.TenRoadMap);
                }
                entity.BiDanh = FuncUtilities.BestLower(entity.BiDanh);

                entity = await _roadMapRepository.InsertAsync(entity);

                //Tạo mới road map detail cho road map mới
                RoadMapDetailViewModel roadMapDetailVm = new RoadMapDetailViewModel();

                roadMapDetailVm.TenRoadMapDetail = entity.TenRoadMap;
                roadMapDetailVm.BiDanh = entity.BiDanh;
                roadMapDetailVm.MaRoadMap = entity.Id;
                
                RoadMapDetail roadMapDetail = _mapper.Map<RoadMapDetail>(roadMapDetailVm);

                //Thêm road map detail mới vào DB
                await _roadMapDetailRepository.InsertAsync(roadMapDetail);


                return new ResponseEntity(StatusCodeConstants.CREATED, entity, MessageConstants.INSERT_SUCCESS);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public override async Task<ResponseEntity> DeleteByIdAsync(List<dynamic> listId)
        {
            try
            {
                IEnumerable<RoadMap> dsRoadMap = await _roadMapRepository.GetMultiByIdAsync(listId);

                if (await _roadMapRepository.DeleteByIdAsync(listId) == 0)
                {
                    return new ResponseEntity(StatusCodeConstants.BAD_REQUEST, listId, MessageConstants.DELETE_ERROR);
                }

                foreach (RoadMap roadMap in dsRoadMap )
                {
                    //Do mỗi cái RoadMap chỉ có 1 cái RoadMapDetail nên lấy ra 1 phần tử, nếu mà có n RoadMapDetail thì sửa lại lấy Multi
                    RoadMapDetail roadMapDetail = await _roadMapDetailRepository.GetSingleByConditionAsync("MaRoadMap", roadMap.Id);

                    List<dynamic> listIdRoadMapDetail = new List<dynamic>();

                    listIdRoadMapDetail.Add(roadMapDetail.Id.ToString());
                    
                    await _roadMapDetailRepository.DeleteByIdAsync(listIdRoadMapDetail);

                }

                return new ResponseEntity(StatusCodeConstants.OK, listId, MessageConstants.DELETE_SUCCESS);
            }
             catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
           
        }

    }

    
}
