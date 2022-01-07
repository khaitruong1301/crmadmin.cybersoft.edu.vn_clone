using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SoloDevApp.Repository.Repositories
{
    public interface IBuoiHoc_NguoiDungRepository : IRepository<BuoiHoc_NguoiDung>
    {
      Task<IEnumerable<BuoiHoc_NguoiDung>> GetTheoDanhSachMaBuoi(List<dynamic> listId);
    }

    public class BuoiHoc_NguoiDungRepository : RepositoryBase<BuoiHoc_NguoiDung>, IBuoiHoc_NguoiDungRepository
    {
        public BuoiHoc_NguoiDungRepository(IConfiguration configuration)
            : base(configuration)
        {

        }

        public async Task<IEnumerable<BuoiHoc_NguoiDung>> GetTheoDanhSachMaBuoi(List<dynamic> listId)
        {
            string lsStringId = "";

            foreach(var item in listId.Select((value, index) => (value, index)))
            {
                if (item.index != 0)
                {   
                    lsStringId = $"{lsStringId} , {item.value}";
                } else
                {
                    lsStringId = $"{item.value}";
                }
                
            }

            lsStringId = $"({lsStringId})";

            string query = $"SELECT * FROM {_table} WHERE MaBuoiHoc IN {lsStringId} AND DaXoa = 0";

            using (var conn = CreateConnection())
            {
                try
                {
                    return await conn.QueryAsync<BuoiHoc_NguoiDung>(query, null, null, null, CommandType.Text);
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
