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

        public List<VideoExtraViewModel> ListVideoExtra { get; set; }
        public List<LopHoc_TaiLieuViewModel> ListLopHoc_TaiLieu { get; set; }
        public List<BaiHocViewModel> ListBaiHoc { get; set; }
        public List<XemLaiBuoiHocViewModel> listXemLaiBuoiHoc { get; set; }
    }
}
