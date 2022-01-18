using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoloDevApp.Api.Filters;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/lophoc")]
    [ApiController]
    //Tạm tắt debug
    //[ApiKeyAuth]
    public class LopHocController : ControllerBase
    {
        private ILopHocService _lopHocService;

        public LopHocController(ILopHocService lopHocService)
        {
            _lopHocService = lopHocService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _lopHocService.GetAllAsync();
        }
        [HttpGet("byyear/{year}")]
        public async Task<IActionResult> LayTheoNam(int year)
        {
            return await _lopHocService.GetClassByYear(year);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _lopHocService.GetSingleByIdAsync(id);
        }

        [HttpGet("byuser/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            return await _lopHocService.GetByUserIdAsync(userId);
        }

        [HttpGet("info/{id}")]
        public async Task<IActionResult> GetInfo(int id)
        {
            return await _lopHocService.GetInfoByIdAsync(id);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            if (keywords!=null && keywords.Trim() != "")
                keywords= keywords.Replace("@@", "%");
            
            return await _lopHocService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("course/{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            return await _lopHocService.GetCourseByClassIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LopHocViewModel model)
        {
            //return await _lopHocService.InsertAsync(model);
            return await _lopHocService.InsertClassAsync(model);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] LopHocViewModel model)
        {
            //return await _lopHocService.UpdateAsync(id, model);
            return await _lopHocService.UpdateClassAsync(id, model);


        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _lopHocService.DeleteByIdClassAsync(Ids);
        }

        [HttpGet("kiem-tra-mentor")]
        public async Task<IActionResult> CheckSoLuongMentor()
        {
            return await _lopHocService.CheckSoLuongMentor();
        }

        //WIKI BEGIN

        [HttpGet("lay-danh-sach-buoi-hoc-theo-lop/{classId}")]
        public async Task<IActionResult> LayDanhSachBuoiHocTheoLop(int classId)
        {
            return await _lopHocService.GetListClassesByClassId(classId);
        }

        [HttpPost("them-buoi-hoc-vao-lop")]
        public async Task<IActionResult> ThemBuoiHocVaoLop(int classId, int classesId)
        {
            return await _lopHocService.AddClassesToClass(classId, classesId);
        }
    }
}