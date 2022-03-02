using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IChiNhanhRepository : IRepository<ChiNhanh>
    {
        Task<NguoiDung> Off_CheckUserFace(string idFace);
        Task<NguoiDung> Off_CheckUserEmail(string email);

    }

    public class ChiNhanhRepository : RepositoryBase<ChiNhanh>, IChiNhanhRepository
    {
        private readonly IConfiguration _configuration;

        public ChiNhanhRepository(IConfiguration configuration)
            : base(configuration)
        {
        }


        public async Task<NguoiDung> Off_CheckUserFace(string idFace)
        {

            string query = $"SELECT * FROM NguoiDung WHERE FacebookId = '{idFace}' AND DaXoa = 0";

            using (var conn = CreateConnection())
            {
                try
                {
                    return await conn.QueryFirstOrDefaultAsync<NguoiDung>(query, null, null, null, CommandType.Text);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
        }

        public async Task<NguoiDung> Off_CheckUserEmail(string email)
        {

            string query = $"SELECT * FROM NguoiDung WHERE Email = '{email}' AND DaXoa = 0";

            using (var conn = CreateConnection())
            {
                try
                {
                    return await conn.QueryFirstOrDefaultAsync<NguoiDung>(query, null, null, null, CommandType.Text);
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