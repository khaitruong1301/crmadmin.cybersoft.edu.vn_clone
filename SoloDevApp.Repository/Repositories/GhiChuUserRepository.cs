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
    public interface IGhiChuUserRepository : IRepository<GhiChuUser>
    {
        Task<IEnumerable<GhiChuUser>> GetTheoMaLop(int maLop);
    }

    public class GhiChuUserRepository : RepositoryBase<GhiChuUser>, IGhiChuUserRepository
    {
        public GhiChuUserRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<IEnumerable<GhiChuUser>> GetTheoMaLop(int maLop)
        {
            string query = $"SELECT * FROM {_table} WHERE MaLop = '{maLop}' AND DaXoa = 0";

            using (var conn = CreateConnection())
            {
                try
                {
                    return await conn.QueryAsync<GhiChuUser>(query, null, null, null, CommandType.Text);
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