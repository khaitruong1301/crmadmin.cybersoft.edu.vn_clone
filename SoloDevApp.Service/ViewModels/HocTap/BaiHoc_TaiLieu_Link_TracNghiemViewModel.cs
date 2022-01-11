using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    public class BaiHoc_TaiLieu_Link_TracNghiemViewModel
    {
        public int Id { get; set; }
        public string TieuDe { get; set; }
        public string BiDanh { get; set; }
        public string NoiDung { get; set; }
        public string MoTa { get; set; }
        public string Vimeo { get; set; }
        public string ThoiLuong { get; set; }
        public string MaBuoi { get; set; }
        public string MaLoaiBaiHoc { get; set; }
        public string GhiChu { get; set; }
    }

    public class TaiLieuViewModel
    {
        public int Id { get; set; }
        public string TieuDe { get; set; }
        public string BiDanh { get; set; }
        public string NoiDung { get; set; }
        public string MoTa { get; set; }
    }

    public class TaiLieuBaiHocViewModel : TaiLieuViewModel { }
    public class TaiLieuBaiTapViewModel : TaiLieuViewModel { }
    public class TaiLieuDocThemViewModel : TaiLieuViewModel { }
    public class TaiLieuProjectLamThemViewModel : TaiLieuViewModel { }
    public class TaiLieuCapstoneViewModel : TaiLieuViewModel {}
    public class TracNghiemViewModel : TaiLieuViewModel { }

    public class VideoFPTViewModel : TaiLieuViewModel {
        public string Vimeo { get; set; }
        public string ThoiLuong { get; set; }
    }

    public class TaiLieuBuoiHocTheoSkillViewModel
    {
        public string TenSkill;
        public List<dynamic> danhSachBaiHoc;
        public TaiLieuBuoiHocTheoSkillViewModel()
        {
            danhSachBaiHoc = new List<dynamic>();
        }
    }

    public class DiemNguoiDungTheoSkillViewModel
    {
        public string TenSkill;
        public List<dynamic> danhSachDiem;
        public DiemNguoiDungTheoSkillViewModel()
        {
            danhSachDiem = new List<dynamic>();
        }
    }



}