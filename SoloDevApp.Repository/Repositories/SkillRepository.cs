using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface ISkillRepository : IRepository<Skill>
    {
    }

    public class SkillRepository : RepositoryBase<Skill>, ISkillRepository
    {
        public SkillRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}