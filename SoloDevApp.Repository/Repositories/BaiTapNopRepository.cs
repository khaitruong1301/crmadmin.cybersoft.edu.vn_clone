using Dapper;
using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SoloDevApp.Repository.Repositories
{
    public interface IBaiTapNopRepository : IRepository<BaiTapNop>
    {
        Task<IEnumerable<BaiTapNop>> GetBaiTapNopTheoLop(int classId);
    }

    public class BaiTapNopRepository : RepositoryBase<BaiTapNop>, IBaiTapNopRepository
    {
        public BaiTapNopRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IEnumerable<BaiTapNop>> GetBaiTapNopTheoLop(int classId)
        {
            string query = $"SELECT * FROM {_table} WHERE MaLopHoc = '{classId}' AND DaXoa = 0";

            using (var conn = CreateConnection())
            {
                try
                {
                    return await conn.QueryAsync<BaiTapNop>(query, null, null, null, CommandType.Text);
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