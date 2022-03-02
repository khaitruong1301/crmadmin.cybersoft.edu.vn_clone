using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/roadmapdetail")]

    [ApiController]
    public class RoadMapDetailController : ControllerBase
    {
        private IRoadMapDetailService _roadMapDetailService;

        public RoadMapDetailController(IRoadMapDetailService roadMapDetailService)
        {
            _roadMapDetailService = roadMapDetailService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _roadMapDetailService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _roadMapDetailService.GetSingleByIdAsync(id);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _roadMapDetailService.GetPagingAsync(page, size, keywords);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RoadMapDetailViewModel model)
        {
            return await _roadMapDetailService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] RoadMapDetailViewModel model)
        {
            return await _roadMapDetailService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _roadMapDetailService.DeleteByIdAsync(Ids);
        }
    }
}