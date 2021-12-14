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
    public interface IQuyenRepository : IRepository<Quyen>
    {
        Task<NguoiDung> Off_CheckUserFace(string idFace);
        Task<NguoiDung> Off_CheckUserEmail(string email);
        Task<NguoiDung> On_CheckUserEmail(string email);

    }

    public class QuyenRepository : RepositoryBase<Quyen>, IQuyenRepository
    {
        private readonly IConfiguration _configuration;

        public QuyenRepository(IConfiguration configuration)
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


        public SqlConnection CreateConnectOnline()
        {
            string _connectionOnline = "Server=103.97.125.205,1433;Database=SoloDev;User Id=khaicybersoft;Password=khaicybersoft321@";

            var conn = new SqlConnection(_connectionOnline);
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            return conn;
        }
        public async Task<NguoiDung> On_CheckUserEmail(string email)
        {

            string query = $"SELECT * FROM tblNguoiDung WHERE email = '{email}' AND DaXoa = 0";

            using (var conn = CreateConnectOnline())
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