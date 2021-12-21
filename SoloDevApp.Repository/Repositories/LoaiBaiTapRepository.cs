using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface ILoaiBaiTapRepository : IRepository<LoaiBaiTap>
    {
    }

    public class LoaiBaiTapRepository : RepositoryBase<LoaiBaiTap>, ILoaiBaiTapRepository
    {
        public LoaiBaiTapRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}