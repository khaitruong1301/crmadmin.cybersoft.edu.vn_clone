﻿namespace SoloDevApp.Repository.Models
{
    public class KhoaHoc
    {
        public int Id { get; set; }
        public string TenKhoaHoc { get; set; }
        public string BiDanh { get; set; }
        public string HinhAnh { get; set; }
        public string TaiLieu { get; set; }
        public string MoTa { get; set; }
        public string DanhSachLoTrinh { get; set; }
        public string DanhSachChuongHoc { get; set; }
        public int SoNgayKichHoat { get; set; }
        public bool KichHoatSan { get; set; }
    }

    public class KhoaHocSkill :KhoaHoc
    {
        public string MaSkil { get; set; }
    }
}