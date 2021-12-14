using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IDiemDanhRepository : IRepository<DiemDanh>
    {
        Task<IEnumerable<DiemDanh>> GetTheoMaLop(int maLop);
    }

    public class DiemDanhRepository : RepositoryBase<DiemDanh>, IDiemDanhRepository
    {
        public DiemDanhRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<DiemDanh>> GetTheoMaLop(int maLop)
        {
            string query = $"SELECT * FROM {_table} WHERE MaLopHoc = '{maLop}' AND DaXoa = 0";

            using (var conn = CreateConnection())
            {
                try
                {
                    return await conn.QueryAsync<DiemDanh>(query, null, null, null, CommandType.Text);
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