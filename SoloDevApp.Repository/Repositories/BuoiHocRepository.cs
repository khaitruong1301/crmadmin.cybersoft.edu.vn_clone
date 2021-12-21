﻿using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IBuoiHocRepository : IRepository<BuoiHoc>
    {
    }

    public class BuoiHocRepository : RepositoryBase<BuoiHoc>, IBuoiHocRepository
    {
        public BuoiHocRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}