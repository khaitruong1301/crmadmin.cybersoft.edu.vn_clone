using System;

namespace SoloDevApp.Repository.Models
{
    public class TrackingNguoiDung
    {
        public int Id { get; set; }
        public bool DaXoa { get; set; }
        public DateTime NgayTao { get; set; }
        public string ChiTietHanhDong { get; set; }
        public string HoTen { get; set; }
        public string MaNguoiDung { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime NgayCuoiCungHoatDong { get; set; }
        public int MaLop { get; set; }
        public string LoaiHanhDong { get; set; }
    }
}