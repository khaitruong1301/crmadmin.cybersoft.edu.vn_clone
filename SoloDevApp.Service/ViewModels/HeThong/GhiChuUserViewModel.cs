using System;
using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    public class GhiChuUserViewModel
    {
        public int Id { get; set; }
        public DateTime NgayTao { get; set; }
        public int MaLop { get; set; }
        public string MaNguoiDung { get; set; }
        public string GhiChu { get; set; }
    }
}