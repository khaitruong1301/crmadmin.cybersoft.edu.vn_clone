using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface ITrackingNguoiDungRepository : IRepository<TrackingNguoiDung>
    {
    }

    public class TrackingNguoiDungRepository : RepositoryBase<TrackingNguoiDung>, ITrackingNguoiDungRepository
    {
        public TrackingNguoiDungRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}