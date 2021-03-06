namespace SoloDevApp.Repository.Models
{
    public class BaiHoc
    {
        public int Id { get; set; }
        public string TenBaiHoc { get; set; }
        public string BiDanh { get; set; }
        public string NoiDung { get; set; }
        public string MaLoaiBaiHoc { get; set; }
        public string DanhSachCauHoi { get; set; }
        public string NoiDungMoTa { get; set; }
        public int MaChuongHoc { get; set; }
        public bool online { get; set; }
        public string Vimeo { get; set; }
    }
}