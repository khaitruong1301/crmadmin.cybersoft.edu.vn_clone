using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IUnitRepository : IRepository<Unit>
    {
    }

    public class UnitRepository : RepositoryBase<Unit>, IUnitRepository
    {
        public UnitRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}