using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.Utilities;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/buoihoc")]
    //[Authorize]
    [ApiController]
    public class BuoiHocController : ControllerBase
    {
        private IBuoiHocService _buoiHocService;

        public BuoiHocController(IBuoiHocService buoiHocService)
        {
            _buoiHocService = buoiHocService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _buoiHocService.GetAllAsync();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _buoiHocService.GetSingleByIdAsync(id);
        }


        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _buoiHocService.GetPagingAsync(page, size, keywords);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BuoiHocViewModel model)
        {
            return await _buoiHocService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BuoiHocViewModel model)
        {
            return await _buoiHocService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _buoiHocService.DeleteByIdAsync(Ids);
        }

        [HttpPost("them-list-buoi-hoc-theo-lop")]
        public async Task<IActionResult> ThemListBuoiHocTheoLop (InputThemListBuoiHocTheoMaLopViewModel model)
        {
            return await _buoiHocService.ThemListBuoiHocTheoMaLop(model);
        }

    }
}