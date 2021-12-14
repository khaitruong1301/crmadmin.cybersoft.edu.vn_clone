﻿using System;

namespace SoloDevApp.Repository.Models
{
    public class LopHoc
    {
        public int Id { get; set; }
        public string TenLopHoc { get; set; }
        public string BiDanh { get; set; }
        public int SoHocVien { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public float HocPhi { get; set; }
        public string ThoiKhoaBieu { get; set; }
        public string DanhSachGiangVien { get; set; }
        public string DanhSachMentor { get; set; }
        public string DanhSachHocVien { get; set; }
        public string DanhSachHocVienNew { get; set; }
        public int MaTrangThai { get; set; }
        public int MaLoTrinh { get; set; }
        public int ChiNhanh { get; set; }
        public string Token { get; set; }
        public int ThuMucVimeo { get; set; }

    }
    public class ThongTinHocVienGhiDanh
    {
        public string Id { get; set; }
        public DateTime ngayGhiDanh { get; set; }
    }
}