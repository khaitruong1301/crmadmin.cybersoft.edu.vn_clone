using System;
using System.Collections.Generic;

namespace SoloDevApp.Service.ViewModels
{
    public class ThongBaoViewModel
    {
       public int Id { get; set; }
       public string MaNguoiDung { get; set; }
       public List<NoiDungThongBaoViewModel> NoiDung { get; set; }
    }

    

    

        public class NoiDungThongBaoViewModel
        {
            public int Id { get; set; }
            public DateTime NgayThang { get; set; }
            public string NoiDung { get; set; }
            public bool DaXem { get; set; }
            public string LoaiThongBao { get; set; }

        }
        public class NoiDungThongBao
        {
            public int Id { get; set; }
            public string NgayThang { get; set; }
            public string NoiDung { get; set; }
            public bool DaXem { get; set; }
            public string LoaiThongBao { get; set; }

        }
}