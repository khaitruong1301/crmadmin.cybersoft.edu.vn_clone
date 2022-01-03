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

        public interface ITaiLieuBaiHocRepository : IRepository<TaiLieuBaiHoc>
        {
        }

        public class TaiLieuBaiHocRepository : RepositoryBase<TaiLieuBaiHoc>, ITaiLieuBaiHocRepository
        {
            public TaiLieuBaiHocRepository(IConfiguration configuration)
                : base(configuration)
            {
            }
        }


        public interface ITaiLieuBaiTapRepository : IRepository<TaiLieuBaiTap>
        {
        }

        public class TaiLieuBaiTapRepository : RepositoryBase<TaiLieuBaiTap>, ITaiLieuBaiTapRepository
        {
            public TaiLieuBaiTapRepository(IConfiguration configuration)
                : base(configuration)
            {
            }
        }

        public interface ITaiLieuDocThemRepository : IRepository<TaiLieuDocThem>
        {
        }

        public class TaiLieuDocThemRepository : RepositoryBase<TaiLieuDocThem>, ITaiLieuDocThemRepository
        {
            public TaiLieuDocThemRepository(IConfiguration configuration)
                : base(configuration)
            {
            }
        }

        public interface ITaiLieuProjectLamThemRepository : IRepository<TaiLieuProjectLamThem>
        {
        }

        public class TaiLieuProjectLamThemRepository : RepositoryBase<TaiLieuProjectLamThem>, ITaiLieuProjectLamThemRepository
        {
            public TaiLieuProjectLamThemRepository(IConfiguration configuration)
                : base(configuration)
            {
            }
        }

        public interface ITracNghiemRepository : IRepository<TracNghiem>
        {
        }

        public class TracNghiemRepository : RepositoryBase<TracNghiem>, ITracNghiemRepository
        {
            public TracNghiemRepository(IConfiguration configuration)
                : base(configuration)
            {
            }
        }

      

    
}

