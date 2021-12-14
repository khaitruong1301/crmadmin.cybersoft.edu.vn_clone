﻿using System;
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
    public interface IXemLaiBuoiHocRepository : IRepository<XemLaiBuoiHoc>
    {
        Task<IEnumerable<XemLaiBuoiHoc>> GetTheoMaLop(int maLop);

    }

    public class XemLaiBuoiHocRepository : RepositoryBase<XemLaiBuoiHoc>, IXemLaiBuoiHocRepository
    {
        public XemLaiBuoiHocRepository(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<IEnumerable<XemLaiBuoiHoc>> GetTheoMaLop(int maLop)
        {
            string query = $"SELECT * FROM {_table} WHERE MaLop = '{maLop}' AND DaXoa = 0";

            using (var conn = CreateConnection())
            {
                try
                {
                    return await conn.QueryAsync<XemLaiBuoiHoc>(query, null, null, null, CommandType.Text);
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