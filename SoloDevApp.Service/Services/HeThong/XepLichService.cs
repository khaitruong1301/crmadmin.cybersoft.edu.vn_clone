using AutoMapper;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;

namespace SoloDevApp.Service.Services
{
    public interface IXepLichService : IService<XepLich, XepLichViewModel>
    {
    }

    public class XepLichService : ServiceBase<XepLich, XepLichViewModel>, IXepLichService
    {
        public XepLichService(IXepLichRepository xepLichRepository, IMapper mapper)
            : base(xepLichRepository, mapper)
        {
        }
    }
}