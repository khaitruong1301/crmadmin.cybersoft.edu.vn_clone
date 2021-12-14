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
    [Route("api/ghichuuser")]
    [ApiController]
   
    public class GhiChuUserController : ControllerBase
    {
        private IGhiChuUserService _ghiChuUserService;

        public GhiChuUserController(IGhiChuUserService ghiChuUserService)
        {
            _ghiChuUserService = ghiChuUserService;
        }

        [HttpGet]   
        public async Task<IActionResult> Get()
        {
            return await _ghiChuUserService.GetAllAsync();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _ghiChuUserService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return await _ghiChuUserService.GetSingleByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GhiChuUserViewModel model)
        {
            return await _ghiChuUserService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] GhiChuUserViewModel model)
        {
            model.NgayTao = DateTime.Now;
            return await _ghiChuUserService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _ghiChuUserService.DeleteByIdAsync(Ids);
        }

        [HttpGet("lay-theo-lop/{maLop}")]
        public async Task<IActionResult> LayTheoMaLop(int maLop)
        {
            return await _ghiChuUserService.LayTheoMaLop(maLop);
        }
    }
}