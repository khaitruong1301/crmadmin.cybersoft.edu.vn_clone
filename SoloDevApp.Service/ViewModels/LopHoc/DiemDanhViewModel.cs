using System;

namespace SoloDevApp.Service.ViewModels
{
    public class DiemDanhViewModel
    {
        public int Id { get; set; }
        public DateTime NgayTao { get; set; }
        public string NguoiDiemDanh { get; set; }
        public string DanhSachHocVien { get; set; }
        public int MaLopHoc { get; set; }

    }
}