using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IVideoExtraRepository : IRepository<VideoExtra>
    {
    }

    public class VideoExtraRepository : RepositoryBase<VideoExtra>, IVideoExtraRepository
    {
        public VideoExtraRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}