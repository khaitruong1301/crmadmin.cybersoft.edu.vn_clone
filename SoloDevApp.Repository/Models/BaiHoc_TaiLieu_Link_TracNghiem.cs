namespace SoloDevApp.Repository.Models
{
    public class BaiHoc_TaiLieu_Link_TracNghiem
    {
        public int Id { get; set; }
        public string TieuDe { get; set; }
        public string BiDanh { get; set; }
        public string NoiDung { get; set; }
        public string MoTa { get; set; }
        public string Vimeo { get; set; }
        public string ThoiLuong { get; set; }
        public string MaBuoi { get; set; }
        public string MaLoaiBaiHoc { get; set; }
        public string GhiChu { get; set; }
    }

    public class TaiLieu
    {
        public int Id { set; get; }
        public string TieuDe { set; get; }
        public string BiDanh { get; set; }
        public string NoiDung { get; set; }
        public string MoTa { get; set; }
        public string GhiChu { set; get; }
    }

    public class Video : TaiLieu
    {
        public string Vimeo { set; get; }
        public string ThoiLuong { get; set; }
    }

    public class TaiLieuBaiHoc : TaiLieu
    {
    }

    public class TaiLieuDocThem : TaiLieu
    {

    }

    public class TaiLieuBaiTap : TaiLieu
        {}
    public class TaiLieuProjectLamThem : TaiLieu
    { }

    public class TracNghiem : TaiLieu
    {
    }
    public class VideoFPT : Video { }
}