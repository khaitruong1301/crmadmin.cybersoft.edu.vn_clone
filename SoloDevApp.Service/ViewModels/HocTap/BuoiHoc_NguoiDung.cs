using System;

namespace SoloDevApp.Service.ViewModels
{
    public class BuoiHoc_NguoiDung
    {
        public int Id { get; set; }
        public string MaNguoiDung { get; set; }
        public int MaBuoiHoc { get; set; }
        public string LichSuHocTap { get; set; }
        public bool DaXoa { get; set; }
        public DateTime NgayTao { get; set; }

    }

}