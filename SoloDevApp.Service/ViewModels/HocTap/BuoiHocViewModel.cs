using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    public class BuoiHocViewModel
    {
        public int Id { get; set; }
        public string TenBuoiHoc { get; set; }
        public string BiDanh { get; set; }
        public int MaRoadMapDetail { get; set; }
        public int STT { get; set; }
        public int MaLop { get; set; }
        public string MaSkill { get; set; }

        // public DateTime NgayHoc { get; set; }



        public List<TaiLieuBaiHocViewModel> TaiLieuBaiHoc { get; set; }
        public List<BaiTapBuoiHocViewModel> TaiLieuBaiTap { get; set; }
        public List<TaiLieuDocThemViewModel> TaiLieuDocThem { get; set; }
        public List<TaiLieuProjectLamThemViewModel> TaiLieuProjectLamThem { get; set; }

        public List<BaiTapBuoiHocViewModel> TaiLieuCapstone { get; set; }
        public List<BaiTapBuoiHocViewModel> TracNghiem { get; set; }
        public List<BaiTapBuoiHocViewModel> TracNghiemExtra { get; set; }

        public List<LichSuHocTapViewModel> LichSuHocTap { get; set; }

        public List<dynamic> VideoXemLai { get; set; }
        public List<dynamic> VideoExtra { get; set; }

        public BuoiHocViewModel()
        {
            LichSuHocTap = new List<LichSuHocTapViewModel>();
            TaiLieuBaiHoc = new List<TaiLieuBaiHocViewModel>();
            TaiLieuBaiTap = new List<BaiTapBuoiHocViewModel>();
            TaiLieuDocThem = new List<TaiLieuDocThemViewModel>();
            TaiLieuProjectLamThem = new List<TaiLieuProjectLamThemViewModel>();
            TaiLieuCapstone = new List<BaiTapBuoiHocViewModel>();
            TracNghiem = new List<BaiTapBuoiHocViewModel>();
            TracNghiemExtra = new List<BaiTapBuoiHocViewModel>();
            VideoXemLai = new List<dynamic>();
            VideoExtra = new List<dynamic>();
        }

    }

    public class BuoiHocBySkillViewModel
    {
        public string TenSkill;
        public List<dynamic> DanhSachKhoaHocBySkill;

        public bool isActive;

        public dynamic DiemBuoiHoc;
        public List<BuoiHocViewModel> DanhSachBuoiHoc { get; set; }

        public BuoiHocBySkillViewModel()
        {
            DanhSachKhoaHocBySkill = new List<dynamic>();
            DanhSachBuoiHoc = new List<BuoiHocViewModel>();
        }
    }

    public class ThongTinBuoiHocTheoLopViewModel
    {
        public List<ThongBaoTrackingViewModel> ThongBao { get; set; }
        public List<dynamic> ThongKeDiemNguoiDung { get; set; }
        public ThongTinLopHocBySkillViewModel ThongTinLopHoc { get; set; }
        public List<BuoiHocBySkillViewModel> DanhSachBuoiHocTheoSkill { get; set; }
        public List<dynamic> DanhSachTaiLieuTheoSkill { get; set; }
        public List<dynamic> DanhSachDiemBaiTapTheoSkill { get; set; }

        public ThongTinBuoiHocTheoLopViewModel()
        {
            ThongBao = new List<ThongBaoTrackingViewModel>();
            ThongKeDiemNguoiDung = new List<dynamic>();
            ThongTinLopHoc = new ThongTinLopHocBySkillViewModel();
            DanhSachBuoiHocTheoSkill = new List<BuoiHocBySkillViewModel>();
            DanhSachTaiLieuTheoSkill = new List<dynamic>();
            DanhSachDiemBaiTapTheoSkill = new List<dynamic>();
        }
    }

    public class InputThemListBuoiHocTheoMaLopViewModel
    {
        public int MaLop { get; set; }
        public int SoBuoiHocCuaLop { get; set; }
        public int MaRoadMapDetail { get; set; } //Có cũng được không có không sao
    }
}
