using System;

namespace SoloDevApp.Repository.Models
{
    public class GhiChuUser
    {
        public int Id { get; set; }
        public DateTime NgayTao { get; set; }
        public int MaLop { get; set; }
        public string MaNguoiDung { get; set; }
        public string GhiChu { get; set; }
    }
}