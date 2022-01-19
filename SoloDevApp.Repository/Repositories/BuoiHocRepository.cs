using Dapper;
using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

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