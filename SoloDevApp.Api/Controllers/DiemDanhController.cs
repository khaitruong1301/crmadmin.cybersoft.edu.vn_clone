using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/diemdanh")]
    [ApiController]
   

    public class DiemDanhController : ControllerBase
    {
        private IDiemDanhService _diemDanhService;

        public DiemDanhController(IDiemDanhService diemDanhService)
        {
            _diemDanhService = diemDanhService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _diemDanhService.GetAllAsync();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _diemDanhService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return await _diemDanhService.GetSingleByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DiemDanhViewModel model)
        {
            return await _diemDanhService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] DiemDanhViewModel model)
        {
            return await _diemDanhService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _diemDanhService.DeleteByIdAsync(Ids);
        }
        [HttpGet("lay-theo-lop/{maLop}")]
        public async Task<IActionResult> LayTheoMaLop(int maLop)
        {
            return await _diemDanhService.LayTheoMaLop(maLop);
        }
    }
}