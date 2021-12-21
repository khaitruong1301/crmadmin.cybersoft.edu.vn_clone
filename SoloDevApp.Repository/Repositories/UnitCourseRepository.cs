using Microsoft.Extensions.Configuration;
using SoloDevApp.Repository.Infrastructure;
using SoloDevApp.Repository.Models;

namespace SoloDevApp.Repository.Repositories
{
    public interface IUnitCourseRepository : IRepository<UnitCourse>
    {
    }

    public class UnitCourseRepository : RepositoryBase<UnitCourse>, IUnitCourseRepository
    {
        public UnitCourseRepository(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}