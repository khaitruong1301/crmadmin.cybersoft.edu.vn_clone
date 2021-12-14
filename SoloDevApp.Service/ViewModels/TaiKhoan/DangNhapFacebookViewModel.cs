using System.ComponentModel.DataAnnotations;

namespace SoloDevApp.Service.ViewModels
{
    public class DangNhapFacebookViewModel
    {
        public string Email { get; set; }

        [Required]
        public string FacebookId { get; set; }

     
        public string FacebookEmail { get; set; }
    }
}