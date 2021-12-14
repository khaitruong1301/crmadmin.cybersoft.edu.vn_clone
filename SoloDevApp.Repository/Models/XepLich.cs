using System;

namespace SoloDevApp.Repository.Models
{
    public class XepLich
    {
        public int Id { get; set; }
        public bool AllDay { get; set; }
        public string Title { get; set; }
        public string GiangVienId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string RRule { get; set; }
    }
}