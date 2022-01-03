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

        public string MaSkill { get; set; }

        

        public List<TaiLieuBaiHocViewModel> TaiLieuBaiHoc { get; set; }
        public List<TaiLieuBaiTapViewModel> TaiLieuBaiTap { get; set; }
        public List<TaiLieuDocThemViewModel> TaiLieuDocThem { get; set; }
        public List<TaiLieuProjectLamThemViewModel> TaiLieuProjectLamThem { get; set; }
        public List<TracNghiemViewModel> TracNghiem { get; set; }

        public List<dynamic> VideoXemLai { get; set; }
        public List<dynamic> VideoExtra { get; set; }

        public BuoiHocViewModel()
        {
            TaiLieuBaiHoc = new List<TaiLieuBaiHocViewModel>();
            TaiLieuBaiTap =   new List<TaiLieuBaiTapViewModel>();
            TaiLieuDocThem = new List<TaiLieuDocThemViewModel>();
            TaiLieuProjectLamThem = new List<TaiLieuProjectLamThemViewModel>();
            TracNghiem = new List<TracNghiemViewModel> ();
            VideoXemLai = new List<dynamic>();
            VideoExtra = new List<dynamic>();
        }

    }

    public class BuoiHocBySkillViewModel
    {
        public string tenSkill;
        public List<BuoiHocViewModel> DanhSachBuoiHoc { get; set;}
    }
}
