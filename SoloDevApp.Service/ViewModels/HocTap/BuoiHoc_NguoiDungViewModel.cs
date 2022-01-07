using System;

namespace SoloDevApp.Service.ViewModels
{
    public class BuoiHoc_NguoiDungViewModel
    {
        public int Id { get; set; }
        public string MaNguoiDung { get; set; }
        public int MaBuoiHoc { get; set; }
        public string LichSuHocTap { get; set; }
        public bool DaXoa { get; set; }
        public DateTime NgayTao { get; set; }

    }

    public class LichSuHocTapViewModel
    {
        public int Id { get; set; }
        public string NoiDung { get; set; }
        public int Diem { get; set; }
        public int MaBaiHoc { get; set; }
        public string NguoiCham { get; set; }
        public string NhanXet { get; set; }
        public string LoaiBaiTap { get; set; }
        public DateTime HanNop { get; set; }
        public DateTime NgayThang { get; set; }
    }

    public class LichSuHocTap
    {
        public int Id { get; set; }
        public string NoiDung { get; set; }
        public int Diem { get; set; }
        public int MaBaiHoc { get; set; }
        public string NguoiCham { get; set; }
        public string NhanXet { get; set; }
        public string LoaiBaiTap { get; set; }
        public string HanNop { get; set; }
        public string NgayThang { get; set; }
    }


    public class thongTinHocTapNguoiDung
    {
        public string MaNguoiDung;
        public int DiemTrongBuoiHoc;
    }


}