using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IXepLichRepository : IRepository<XepLich>
    {
    }

    public class XepLichRepository : RepositoryBase<XepLich>, IXepLichRepository
    {
        public XepLichRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}