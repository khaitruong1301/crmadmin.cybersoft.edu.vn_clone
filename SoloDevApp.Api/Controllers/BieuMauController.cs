using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/bieumau")]
    [ApiController]


    public class BieuMauController : ControllerBase
    {
        private IBieuMauService _bieuMauService;

        public BieuMauController(IBieuMauService bieuMauService)
        {
            _bieuMauService = bieuMauService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _bieuMauService.GetAllAsync();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _bieuMauService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return await _bieuMauService.GetSingleByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BieuMauViewModel model)
        {
            return await _bieuMauService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] BieuMauViewModel model)
        {
            return await _bieuMauService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _bieuMauService.DeleteByIdAsync(Ids);
        }
    }
}