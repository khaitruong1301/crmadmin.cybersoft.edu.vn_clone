using AutoMapper;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;
using System;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface IChungChiService : IService<ChungChi, ChungChiViewModel>
    {
        Task<ResponseEntity> ThemChungChi(ChungChiViewModel model);
    }

    public class ChungChiService : ServiceBase<ChungChi, ChungChiViewModel>, IChungChiService
    {
        private IChungChiRepository _chungChiRepository;
        public ChungChiService(IChungChiRepository chungChiRepository, IMapper mapper)
            : base(chungChiRepository, mapper)
        {
            _chungChiRepository = chungChiRepository;
        }


        public async Task<ResponseEntity> ThemChungChi(ChungChiViewModel model)
        {
            try
            {
                DateTime dNow = DateTime.Now;
                ChungChi modelMain = await _chungChiRepository.GetTheoLop_UserChungChi(model.MaLop, model.MaNguoiDung);
                if (modelMain == null)
                {
                    modelMain = new ChungChi();
                    modelMain.NgayTao = model.NgayTao;
                    modelMain.MaLop = model.MaLop;
                    modelMain.MaNguoiDung = model.MaNguoiDung;
                    modelMain.Diem = model.Diem;
                    modelMain.ThoiGianDaoTao = model.ThoiGianDaoTao;

                    await _chungChiRepository.InsertAsync(modelMain);

                     modelMain = await _chungChiRepository.GetTheoLop_UserChungChi(model.MaLop, model.MaNguoiDung);
                    return new ResponseEntity(StatusCodeConstants.OK, modelMain);

                }
                else
                    return new ResponseEntity(StatusCodeConstants.OK, modelMain);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }
    }
}