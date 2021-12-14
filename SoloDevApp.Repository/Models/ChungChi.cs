using System;

namespace SoloDevApp.Repository.Models
{
    public class ChungChi
    {
        public int Id { get; set; }
        public int MaLop { get; set; }
        public string MaNguoiDung { get; set; }
        public DateTime NgayTao { get; set; }
        public float Diem { get; set; }
        public int ThoiGianDaoTao { get; set; }
    }
}