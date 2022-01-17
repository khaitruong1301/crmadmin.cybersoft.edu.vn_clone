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
    [Route("api/tracking-nguoi-dung")]
    [ApiController]
    public class TrackingNguoiDungController : ControllerBase
    {
        private ITrackingNguoiDungService _trackingNguoiDungService;

        public TrackingNguoiDungController(ITrackingNguoiDungService trackingNguoiDungService)
        {
            _trackingNguoiDungService = trackingNguoiDungService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _trackingNguoiDungService.GetAllAsync();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _trackingNguoiDungService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _trackingNguoiDungService.GetSingleByIdAsync(id);
        }

        //[HttpGet("lay-thong-bao")]
        //public async Task<IActionResult> LayThongBaoNguoiDung()
        //{
        //    return await _trackingNguoiDungService.getThongBaoNguoiDung();
        //}

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TrackingNguoiDungViewModel model)
        {
            return await _trackingNguoiDungService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TrackingNguoiDungViewModel model)
        {
            return await _trackingNguoiDungService.UpdateAsync(id, model);
        }


        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _trackingNguoiDungService.DeleteByIdAsync(Ids);
        }


    }

}