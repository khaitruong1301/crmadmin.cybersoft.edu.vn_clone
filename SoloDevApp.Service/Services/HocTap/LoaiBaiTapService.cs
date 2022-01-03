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
    public interface ILoaiBaiTapService : IService<LoaiBaiTap, LoaiBaiTapViewModel>
    {

    }

    public class LoaiBaiTapService : ServiceBase<LoaiBaiTap, LoaiBaiTapViewModel>, ILoaiBaiTapService
    {
        ILoaiBaiTapRepository _loaiBaiTapRepository;
        public LoaiBaiTapService(ILoaiBaiTapRepository loaiBaiTapRepository,
            IMapper mapper)
            : base(loaiBaiTapRepository, mapper)
        {
            _loaiBaiTapRepository = loaiBaiTapRepository;

        }
    }
}