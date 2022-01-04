using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IKhoaHocRepository : IRepository<KhoaHoc>
    {
    }

    public class KhoaHocRepository : RepositoryBase<KhoaHoc>, IKhoaHocRepository
    {
        public KhoaHocRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }

    public interface IKhoaHocSkillRepository : IRepository<KhoaHocSkill>
    {

    }
    public class KhoaHocSkillRepository : RepositoryBase<KhoaHocSkill>, IKhoaHocSkillRepository
    {
        public KhoaHocSkillRepository (IConfiguration configuration) : base (configuration)
        {

        }
    }
}