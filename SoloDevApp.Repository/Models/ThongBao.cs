using System;

namespace SoloDevApp.Repository.Models
{
    public class ThongBao
    {
        public int Id { get; set; }
        public DateTime NgayTao { get; set; }
        public bool DaXoa { get; set; }
        public string NoiDung { get; set; }
        public string MaNguoiDung { get; set; }

    }

}