using System;
using System.ComponentModel.DataAnnotations;

namespace SoloDevApp.Service.ViewModels
{
    public class TrackingNguoiDungViewModel
    {
        public int Id { get; set; }
        public string ChiTietHanhDong { get; set; }
        public string HoTen { get; set; }
        public string MaNguoiDung { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime NgayCuoiCungHoatDong { get; set; }
        public int MaLop { get; set; }
        public string LoaiHanhDong { get; set; }
    }

    public class ThongBaoTrackingViewModel
    {
        public DateTime NgayThang { get; set; }
        public string NoiDung { get; set; }
        public string LoaiThongBao { get; set; }

        public ThongBaoTrackingViewModel(string _noiDung, string _loaiThongBao, DateTime _ngayThang)
        {
            NoiDung = _noiDung;
            LoaiThongBao = _loaiThongBao;
            NgayThang = _ngayThang;
        }
    }

    public class ChiTietHanhDongBaseViewModel
    {
        [Required]
        public DateTime NgayThang { get; set; }
        public string MaBaiHoc { get; set; }
        public string TieuDe { get; set; }

        
    }

    public class ChiTietHanhDongBaiTapViewModel : ChiTietHanhDongBaseViewModel
    {
        public DateTime NgayHetHan {get; set; }
        public bool DaNop { get; set; }
        public bool DaCham { get; set; }
        public int Diem { get; set; }
    }

    public class ChiTietHanhDongVideoXemLaiViewModel
    {
        public int IdVideo { get; set; }
        public string TieuDe { get; set; }
        public DateTime NgayHetHan { get; set; }
        public bool DaXem { get; set; }
        public DateTime NgayThang { get; set; }
    }

    public class ChiTietHanhDongUpBaiTapViewModel : ChiTietHanhDongBaseViewModel
    {
        public DateTime NgayHetHan { set; get; }
        public string LoaiBaiTap { get; set; }

    }

    public class ChiTietHanhDongUpTaiLieuViewModel :ChiTietHanhDongBaseViewModel { }

    public class ChiTietHanhDongUpVideoViewModel
    {
        public string TieuDe { set; get; }
        public DateTime NgayThang { set; get; }
    }
}