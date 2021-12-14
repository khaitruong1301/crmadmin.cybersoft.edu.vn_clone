using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/chinhanh")]
    [ApiController]
   

    public class ChiNhanhController : ControllerBase
    {
        private IChiNhanhService _chiNhanhService;

        public ChiNhanhController(IChiNhanhService chiNhanhService)
        {
            _chiNhanhService = chiNhanhService;
        }

        [HttpGet]
        //[Authorize(Roles = "VIEW_ROLE")]
        public async Task<IActionResult> Get()
        {
            return await _chiNhanhService.GetAllAsync();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _chiNhanhService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "ADD_ROLE")]
        public async Task<IActionResult> Get(string id)
        {
            return await _chiNhanhService.GetSingleByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChiNhanhViewModel model)
        {
            return await _chiNhanhService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] ChiNhanhViewModel model)
        {
            return await _chiNhanhService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _chiNhanhService.DeleteByIdAsync(Ids);
        }
    }
}