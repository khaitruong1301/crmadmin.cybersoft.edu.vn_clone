﻿namespace SoloDevApp.Repository.Models
{
    public class BaiTap
    {
        public int Id { get; set; }
        public string TenBaiTap { get; set; }
        public string BiDanh { get; set; }
        public string NoiDung { get; set; }
        public int SoNgayKichHoat { get; set; }
        public int MaLoTrinh { get; set; }
        public string GhiChu { get; set; }
        public int HanNop { get; set; }
        public bool TaiLieu { get; set; }
    }
}