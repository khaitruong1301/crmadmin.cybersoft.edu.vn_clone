using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IBuoiHoc_NguoiDungRepository : IRepository<BuoiHoc_NguoiDung>
    {
    }

    public class BuoiHoc_NguoiDungRepository : RepositoryBase<BuoiHoc_NguoiDung>, IBuoiHoc_NguoiDungRepository
    {
        public BuoiHoc_NguoiDungRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}
