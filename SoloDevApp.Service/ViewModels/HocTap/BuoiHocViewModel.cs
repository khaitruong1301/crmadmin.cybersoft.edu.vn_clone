using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    public class BuoiHocViewModel
    {
        public int Id { get; set; } 
        public string TenBuoiHoc { get; set; }
        public string BiDanh { get; set; }
        public int MaRoadMapDetail { get; set; }
        public string DanhSachBaiHocTracNghiem { get; set; }
        public int STT { get; set; }
        public int MaLop { get; set; }
        public List<dynamic> BaiHoc { get; set; }
        public List<dynamic> TaiLieu { get; set; }
        public List<dynamic> BaiHocVideoFPT { get; set; }
        public List<dynamic> BaiTapNop { get; set; }
        public List<dynamic> VideoXemLai { get; set; }
        public List<dynamic> VideoExtra { get; set; }

        public BuoiHocViewModel()
        {
            BaiHoc = new List<dynamic>();
            TaiLieu =   new List<dynamic>();
            BaiTapNop = new List<dynamic>();
            VideoXemLai = new List<dynamic>();
            BaiHocVideoFPT = new List<dynamic>();
            VideoExtra = new List<dynamic>();
        }

    }
}
