using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Api.Filters;
using SoloDevApp.Repository.Models;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/nguoidung")]
    [ApiController]
    [ApiKeyAuth]

    public class NguoiDungController : ControllerBase
    {
        private INguoiDungService _nguoiDungService;

        public NguoiDungController(INguoiDungService nguoiDungService, IUploadService uploadService)
        {
            _nguoiDungService = nguoiDungService;
        }
 
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _nguoiDungService.GetAllAsync();
        }
        
        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            if (keywords != null && keywords.Trim() != "")
                keywords = keywords.Replace("@@", "%");

            return await _nguoiDungService.GetPagingAsync(page, size, keywords);
        }
  
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return await _nguoiDungService.GetSingleByIdAsync(id);
        }
    
        [HttpGet("{column}/{values}")]
        public async Task<IActionResult> Get(string column, string values)
        {
            List<dynamic> listId = JsonConvert.DeserializeObject<List<dynamic>>(values);
            return await _nguoiDungService.GetByRoleGroupAsync(column, listId);
        }
    
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DangKyViewModel model)
        {
            return await _nguoiDungService.InsertUserAsync(model);
        }
 
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] DangKyViewModel model)
        {
            return await _nguoiDungService.SignUpAsync(model);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] DangNhapFacebookViewModel model)
        {
            return await _nguoiDungService.SignInFacebookAsync(model);
        }

        [HttpPost("admin-login")]
        public async Task<IActionResult> AdminLogin([FromBody] DangNhapViewModel model)
        {
            return await _nguoiDungService.SignInAsync(model);
        }

        [HttpPut("password/{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] DoiMatKhauViewModel model)
        {
            return await _nguoiDungService.ChangePasswordAsync(model);
        }
   
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] SuaNguoiDungViewModel model)
        {
            return await _nguoiDungService.UpdateUserAsync(id, model);
        }
      
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _nguoiDungService.DeleteByIdAsync(Ids);
        }

        [HttpPost("check-pass")]
        public async Task<IActionResult> CheckPass(NguoiDung model)
        {
            return await _nguoiDungService.CheckPass(model);
        }

        [HttpPost("exportexcel")]
        public async Task<IActionResult> ExportToExcel()
        {
            return await _nguoiDungService.ExportExel();

        }
    }
}