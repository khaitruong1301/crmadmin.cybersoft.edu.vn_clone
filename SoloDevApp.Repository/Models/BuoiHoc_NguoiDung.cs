using System;

namespace SoloDevApp.Repository.Models
{
    public class BuoiHoc_NguoiDung
    {
        public int Id { get; set; }
        public string MaNguoiDung { get; set; }
        public int MaBuoiHoc { get; set; }
        public string LichSuHocTap { get; set; }
        public bool DaXoa { get; set; }
        public string NgayTao { get; set; }

    }
}