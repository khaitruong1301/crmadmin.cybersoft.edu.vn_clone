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
    public interface ILopHoc_TaiLieuRepository : IRepository<LopHoc_TaiLieu>
    {
        
           Task<IEnumerable<LopHoc_TaiLieu>> GetTheoMaLop(int maLop);
        Task<int> DeleteLopTaiLieu(int id);
        Task<LopHoc_TaiLieu> GetTheoMaLop_MaTaiLieu(int maLop, int maTaiLieu);
    }

    public class LopHoc_TaiLieuRepository : RepositoryBase<LopHoc_TaiLieu>, ILopHoc_TaiLieuRepository
    {
        public LopHoc_TaiLieuRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<LopHoc_TaiLieu> GetTheoMaLop_MaTaiLieu(int maLop, int maTaiLieu)
        {
            string query = $"SELECT * FROM {_table} WHERE MaLop = '{maLop}' AND MaBaiTap = '{maTaiLieu}' AND DaXoa = 0";

            using (var conn = CreateConnection())
            {
                try
                {
                    return await conn.QueryFirstOrDefaultAsync<LopHoc_TaiLieu>(query, null, null, null, CommandType.Text);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
        }

        public async Task<IEnumerable<LopHoc_TaiLieu>> GetTheoMaLop(int maLop)
        {
            string query = $"SELECT * FROM {_table} WHERE MaLop = '{maLop}' AND DaXoa = 0";

            using (var conn = CreateConnection())
            {
                try
                {
                    return await conn.QueryAsync<LopHoc_TaiLieu>(query, null, null, null, CommandType.Text);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
        }
        public async Task<int> DeleteLopTaiLieu(int id)
        {
            string query = $"DELETE FROM {_table} WHERE id = {id}";

            using (var conn = CreateConnection())
            {
                try
                {
                    return await conn.ExecuteAsync(query, null, null, null, CommandType.Text);
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