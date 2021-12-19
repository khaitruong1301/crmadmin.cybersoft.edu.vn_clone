using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Api.Filters;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.Services.HocTap;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/roadmap")]
    [ApiController]
    //[ApiKeyAuth]
    public class RoadMapController : ControllerBase
    {
        private IRoadMapService _roadMapService;

        public RoadMapController(IRoadMapService roadMapService)
        {
            _roadMapService = roadMapService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _roadMapService.GetAllAsync();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _roadMapService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _roadMapService.GetSingleByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RoadMapViewModel model)
        {
            return await _roadMapService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BaiHocViewModel model)
        {
            return await _roadMapService.UpdateAsync(id, model);
        }

        [HttpPut("question/{id}")]
        public async Task<IActionResult> Question(int id, [FromBody] CauHoiViewModel model)
        {
            return await _baiHocService.AddQuestionToLessonAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _baiHocService.DeleteByIdAsync(Ids);
        }
    }
}