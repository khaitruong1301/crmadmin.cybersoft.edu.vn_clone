using System;

namespace SoloDevApp.Repository.Models
{
    public class HocPhi
    {
        public int Id { get; set; }
        public string MaKH { get; set; }
        public float TongTien { get; set; }
        public string Dot1 { get; set; }
        public string Dot2 { get; set; }
        public string Dot3 { get; set; }
        public string Dot4 { get; set; }
        public DateTime NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public int KhoaHoc { get; set; }
        public string ThongTinNhapHoc { get; set; }
        public bool Online { get; set; }
    }
}