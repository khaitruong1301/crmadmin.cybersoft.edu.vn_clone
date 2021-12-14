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
    public interface IChungChiRepository : IRepository<ChungChi>
    {
        Task<ChungChi> GetTheoLop_UserChungChi(int maLop, string maNguoiDung);
    }

    public class ChungChiRepository : RepositoryBase<ChungChi>, IChungChiRepository
    {
        public ChungChiRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ChungChi> GetTheoLop_UserChungChi(int maLop, string maNguoiDung)
        {
            string query = $"SELECT * FROM {_table} WHERE MaLop = '{maLop}' AND MaNguoiDung = '{maNguoiDung}' AND DaXoa = 0";

            using (var conn = CreateConnection())
            {
                try
                {
                    return await conn.QueryFirstOrDefaultAsync<ChungChi>(query, null, null, null, CommandType.Text);
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