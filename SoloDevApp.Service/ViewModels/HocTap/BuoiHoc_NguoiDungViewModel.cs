using System;

namespace SoloDevApp.Service.ViewModels
{
    public class BuoiHoc_NguoiDungViewModel
    {
        public int Id { get; set; }
        public string MaNguoiDung { get; set; }
        public int MaBuoiHoc { get; set; }
        public string LichSuHocTap { get; set; }


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


    public class ThongTinHocTapNguoiDungViewModel
    {
        public string MaNguoiDung;
        public int DiemTrongBuoiHoc;
    }

    public class DiemBaiTapViewModel
    {
        public string TieuDe;
        public int DiemBaiTap;
        public string LoaiBaiTap;
    }

    public class ThongTinNopBaiTapViewModelBase
    {
        public int MaBuoiHoc;
        public string MaNguoiDung;
        public int MaBaiHoc;
        public string LoaiBaiTap;
    }

    public class ThongTinNopBaiTapTracNghiemViewModel : ThongTinNopBaiTapViewModelBase
    {
        public int Diem;
    }

    public class ThongTinNopBaiTapViewModel : ThongTinNopBaiTapViewModelBase
    {
        public string NoiDung;
    }

    public class ThongTinNopBaiTapCapstone : ThongTinNopBaiTapViewModelBase
    {
        public ThongTinNopCapstoneProjectViewModel NoiDung;
    }

    public class ThongTinNopCapstoneProjectViewModel
    {
        public string LinkBai;
        public string LinkYoutube;
        public string LinkDeploy;
    }


}