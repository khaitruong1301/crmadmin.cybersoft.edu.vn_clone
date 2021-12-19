using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IRoadMapRepository : IRepository<RoadMap>
    {

    }

    public class RoadMapRepository : RepositoryBase<RoadMap>, IRoadMapRepository
    {
        public RoadMapRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}