using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoloDevApp.Service.ViewModels
{
    public class RoadMapDetailViewModel
    { 
       public int Id { get; set; }
       public string TenRoadMapDetail { get; set; }
       public string BiDanh { get; set; }
       public int STT { get; set; }
       public string  DanhSachSkill { get; set; }
       public bool EndRoadMap { get; set; }
       public int MaRoadMap { get; set; }
    }
}
