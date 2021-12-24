using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IBaiHoc_TaiLieu_Link_TracNghiemRepository : IRepository<BaiHoc_TaiLieu_Link_TracNghiem>
    {
    }

    public class BaiHoc_TaiLieu_Link_TracNghiemRepository : RepositoryBase<BaiHoc_TaiLieu_Link_TracNghiem>, IBaiHoc_TaiLieu_Link_TracNghiemRepository
    {
        public BaiHoc_TaiLieu_Link_TracNghiemRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}

