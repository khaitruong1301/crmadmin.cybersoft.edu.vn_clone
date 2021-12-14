using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IBieuMauRepository : IRepository<BieuMau>
    {
    }

    public class BieuMauRepository : RepositoryBase<BieuMau>, IBieuMauRepository
    {
        public BieuMauRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}