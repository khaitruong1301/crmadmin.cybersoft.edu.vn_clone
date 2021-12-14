using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Api.Filters;
using SoloDevApp.Repository.Models;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/lophoc_tailieu")]
    [ApiController]
    [ApiKeyAuth]
    public class LopHoc_TaiLieuController : ControllerBase
    {
        private ILopHoc_TaiLieuService _lopHoc_TaiLieuService;

        public LopHoc_TaiLieuController(ILopHoc_TaiLieuService lopHoc_TaiLieuService)
        {
            _lopHoc_TaiLieuService = lopHoc_TaiLieuService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _lopHoc_TaiLieuService.GetAllAsync();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _lopHoc_TaiLieuService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return await _lopHoc_TaiLieuService.GetSingleByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LopHoc_TaiLieuViewModel model)
        {
         
            return await _lopHoc_TaiLieuService.ThemLopTaiLieu(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] LopHoc_TaiLieuViewModel model)
        {
            return await _lopHoc_TaiLieuService.UpdateAsync(id, model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _lopHoc_TaiLieuService.DeleteByIdAsync(Ids);
        }

        [HttpGet("lay-theo-lop/{maLop}")]
        public async Task<IActionResult> LayTheoMaLop(int maLop)
        {
            return await _lopHoc_TaiLieuService.LayTheoMaLop(maLop);
        }

        [HttpDelete("xoa-lop-tai-lieu")]
        public async Task<IActionResult> XoaLopTaiLieu([FromBody] LopHoc_TaiLieu model)
        {
            return await _lopHoc_TaiLieuService.XoaLopTaiLieu(model);
        }
    }
}