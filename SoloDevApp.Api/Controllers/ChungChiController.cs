using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/chungchi")]
    [ApiController]
   

    public class ChungChiController : ControllerBase
    {
        private IChungChiService _chungChiService;

        public ChungChiController(IChungChiService chungChiService)
        {
            _chungChiService = chungChiService;
        }

        [HttpGet]
        //[Authorize(Roles = "VIEW_ROLE")]
        public async Task<IActionResult> Get()
        {
            return await _chungChiService.GetAllAsync();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _chungChiService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "ADD_ROLE")]
        public async Task<IActionResult> Get(string id)
        {
            return await _chungChiService.GetSingleByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChungChiViewModel model)
        {
            return await _chungChiService.ThemChungChi(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] ChungChiViewModel model)
        {
            return await _chungChiService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _chungChiService.DeleteByIdAsync(Ids);
        }
    }
}