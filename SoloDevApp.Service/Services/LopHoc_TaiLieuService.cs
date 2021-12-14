using AutoMapper;
using Newtonsoft.Json;
using SoloDevApp.Repository.Models;
using SoloDevApp.Repository.Repositories;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Infrastructure;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SoloDevApp.Service.Services
{
    public interface ILopHoc_TaiLieuService : IService<LopHoc_TaiLieu, LopHoc_TaiLieuViewModel>
    {
     
           Task<ResponseEntity> LayTheoMaLop(int maLop);
        Task<ResponseEntity> XoaLopTaiLieu(LopHoc_TaiLieu model);

        Task<ResponseEntity> ThemLopTaiLieu(LopHoc_TaiLieuViewModel model);
    }

    public class LopHoc_TaiLieuService : ServiceBase<LopHoc_TaiLieu, LopHoc_TaiLieuViewModel>, ILopHoc_TaiLieuService
    {
        private ILopHoc_TaiLieuRepository _lopHoc_TaiLieuRepository;
        public LopHoc_TaiLieuService(ILopHoc_TaiLieuRepository lopHoc_TaiLieuRepository, IMapper mapper)
            : base(lopHoc_TaiLieuRepository, mapper)
        {
            _lopHoc_TaiLieuRepository = lopHoc_TaiLieuRepository;
        }

        public async Task<ResponseEntity> ThemLopTaiLieu(LopHoc_TaiLieuViewModel model)
        {
            try
            {
                DateTime dNow = DateTime.Now;
                LopHoc_TaiLieu modelMain = await _lopHoc_TaiLieuRepository.GetTheoMaLop_MaTaiLieu(model.MaLop, model.MaBaiTap);
                if (modelMain == null || model.MaBaiTap==0)
                {
                    modelMain = new LopHoc_TaiLieu();
                    modelMain.NgayTao = dNow;
                    modelMain.MaBaiTap = model.MaBaiTap;
                    modelMain.MaLop = model.MaLop;
                    modelMain.ThuTuBuoi = model.ThuTuBuoi;
                    modelMain.NgayHetHan = dNow.AddDays(model.SoNgayHetHan);
                    modelMain.DuongDan = model.DuongDan;
                    modelMain.TaiLieu = model.TaiLieu;
                    modelMain.NoiDung = model.NoiDung;

                    await _lopHoc_TaiLieuRepository.InsertAsync(modelMain);
                    return new ResponseEntity(StatusCodeConstants.OK, modelMain);

                }
                else
                    return new ResponseEntity(StatusCodeConstants.OK, "1");
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }

        public async Task<ResponseEntity> LayTheoMaLop(int maLop)
        {
            try
            {
                DateTime dNow = DateTime.Now;

                IEnumerable<LopHoc_TaiLieu> dsLopHoc_TaiLieu = await _lopHoc_TaiLieuRepository.GetTheoMaLop(maLop);

                List<LopHoc_TaiLieu_CoHanViewModel> lstLopHoc_TaiLieu = new List<LopHoc_TaiLieu_CoHanViewModel>();

                // kiem tra thoi han khoa hoc
                foreach(var item in dsLopHoc_TaiLieu)
                {

                    LopHoc_TaiLieu_CoHanViewModel itemLop_TaiLieu = new LopHoc_TaiLieu_CoHanViewModel();
                    itemLop_TaiLieu.Id=item.Id;
                    itemLop_TaiLieu.NgayTao = item.NgayTao;
                    itemLop_TaiLieu.MaBaiTap = item.MaBaiTap;
                    itemLop_TaiLieu.MaLop = item.MaLop;
                    itemLop_TaiLieu.NgayHetHan = item.NgayHetHan;
                    itemLop_TaiLieu.ThuTuBuoi = item.ThuTuBuoi;
                    itemLop_TaiLieu.HetHan = false;
                    itemLop_TaiLieu.DuongDan = item.DuongDan;
                    itemLop_TaiLieu.TaiLieu = item.TaiLieu;
                    itemLop_TaiLieu.GhiChu = item.GhiChu;
                    itemLop_TaiLieu.NoiDung = item.NoiDung;

                    if (item.NgayHetHan.Date < dNow.Date)
                        itemLop_TaiLieu.HetHan = true;

                    lstLopHoc_TaiLieu.Add(itemLop_TaiLieu);
                }

        
                return new ResponseEntity(StatusCodeConstants.OK, lstLopHoc_TaiLieu);
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }


        public async Task<ResponseEntity> XoaLopTaiLieu(LopHoc_TaiLieu model)
        {
            try
            {

                 await _lopHoc_TaiLieuRepository.DeleteLopTaiLieu(model.Id);

                if (model.MaBaiTap == 0)
                {
                    dynamic duongDan = JsonConvert.DeserializeObject(model.DuongDan);
                    string fileUrl = duongDan[1];
                    bool bCheck = false;

                    //duyet de xoa file trong thu muc
                    IEnumerable<LopHoc_TaiLieu> lopTaiLieu = await _lopHoc_TaiLieuRepository.GetAllAsync();
                    foreach (var item in lopTaiLieu)
                    {
                        if (item.DuongDan.Contains(fileUrl))
                        {
                            bCheck = true;
                        }
                        if (bCheck == true)
                            break;
                    }

                    if (bCheck == false)
                    {
                        //fileUrl = fileUrl.Replace("/", "\\");
                        string pathFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        var path = @pathFolder + @fileUrl;

                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                }
               
                return new ResponseEntity(StatusCodeConstants.OK, "1");
            }
            catch (Exception ex)
            {
                return new ResponseEntity(StatusCodeConstants.ERROR_SERVER, ex.Message);
            }
        }
    }
}