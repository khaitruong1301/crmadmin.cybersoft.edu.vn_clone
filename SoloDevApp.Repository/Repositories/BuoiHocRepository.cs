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
        Task<dynamic> GetTheoLop_BuoiHoc(int maLop);
    }

    public class BuoiHocRepository : RepositoryBase<BuoiHoc>, IBuoiHocRepository
    {
        public BuoiHocRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<dynamic> GetTheoLop_BuoiHoc(int maLop)
        {
            using (var conn = CreateConnection())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@classId", maLop);
                    parameters.Add("@BuoiHocQuery", null, DbType.Object, ParameterDirection.Output);
                    return await conn.QueryAsync<HocPhi>("GET_BUOI_HOC_BY_CLASS_ID", parameters, null, null, CommandType.StoredProcedure);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }

        }
    }
}