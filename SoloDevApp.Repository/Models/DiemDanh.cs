using System;

namespace SoloDevApp.Repository.Models
{
    public class DiemDanh
    {
        public int Id { get; set; }
        public DateTime NgayTao { get; set; }
        public string NguoiDiemDanh { get; set; }
        public string DanhSachHocVien { get; set; }
        public int MaLopHoc { get; set; }
    }
}