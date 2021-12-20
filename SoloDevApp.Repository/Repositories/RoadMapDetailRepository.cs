using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IRoadMapDetailRepository : IRepository<RoadMapDetail>
    {

    }

    public class RoadMapDetailRepository : RepositoryBase<RoadMapDetail>, IRoadMapDetailRepository
    {
        public RoadMapDetailRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}