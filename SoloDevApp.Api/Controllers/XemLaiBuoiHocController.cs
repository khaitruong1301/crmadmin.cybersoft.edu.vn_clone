using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Api.Filters;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/xemlaibuoihoc")]
    [ApiController]
    [ApiKeyAuth]

    public class XemLaiBuoiHocController : ControllerBase
    {
        private IXemLaiBuoiHocService _xemLaiBuoiHocService;

        public XemLaiBuoiHocController(IXemLaiBuoiHocService xemLaiBuoiHocService)
        {
            _xemLaiBuoiHocService = xemLaiBuoiHocService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _xemLaiBuoiHocService.GetAllAsync();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _xemLaiBuoiHocService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return await _xemLaiBuoiHocService.GetSingleByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] XemLaiBuoiHocViewModel model)
        {
            return await _xemLaiBuoiHocService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] XemLaiBuoiHocViewModel model)
        {
            return await _xemLaiBuoiHocService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _xemLaiBuoiHocService.DeleteByIdAsync(Ids);
        }
        [HttpGet("lay-theo-lop/{malop}")]
        public async Task<IActionResult> Delete(int malop)
        {
            return await _xemLaiBuoiHocService.GetTheoLop(malop);
        }
    }
}