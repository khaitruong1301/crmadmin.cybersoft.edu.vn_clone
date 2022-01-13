using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface ITrackingNguoiDungRepository : IRepository<TrackingNguoiDungRepository>
    {
    }

    public class TrackingNguoiDungRepository : RepositoryBase<TrackingNguoiDungRepository>, ITrackingNguoiDungRepository
    {
        public TrackingNguoiDungRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}