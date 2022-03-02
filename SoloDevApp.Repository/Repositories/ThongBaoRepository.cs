using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IThongBaoRepository : IRepository<ThongBao>
    {
    }

    public class ThongBaoRepository : RepositoryBase<ThongBao>, IThongBaoRepository
    {
        public ThongBaoRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}