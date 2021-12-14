using System;
using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    public class LopHoc_TaiLieu_CoHanViewModel
    {
        public int Id { get; set; }
        public DateTime NgayTao { get; set; }
        public int MaBaiTap { get; set; }
        public int MaLop { get; set; }
        public DateTime NgayHetHan { get; set; }
        public int ThuTuBuoi { get; set; }
        public bool HetHan { get; set; }
        public bool TaiLieu { get; set; }
        public string DuongDan { get; set; }
        public string GhiChu { get; set; }
        public string NoiDung { get; set; }
    }
}