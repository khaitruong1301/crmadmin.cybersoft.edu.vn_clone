using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/xeplich")]
    [ApiController]


    public class XepLichController : ControllerBase
    {
        private IXepLichService _xepLichService;

        public XepLichController(IXepLichService xepLichService)
        {
            _xepLichService = xepLichService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _xepLichService.GetAllAsync();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _xepLichService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return await _xepLichService.GetSingleByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] XepLichViewModel model)
        {
            
            return await _xepLichService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] XepLichViewModel model)
        {
            return await _xepLichService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _xepLichService.DeleteByIdAsync(Ids);
        }
    }
}