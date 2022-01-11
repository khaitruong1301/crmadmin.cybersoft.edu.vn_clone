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
    [Route("api/buoihoc-nguoidung")]
    [ApiController]

    public class BuoiHoc_NguoiDungController : ControllerBase
    {
        private IBuoiHoc_NguoiDungService _buoiHoc_NguoiDungService;

        public BuoiHoc_NguoiDungController(IBuoiHoc_NguoiDungService buoiHoc_NguoiDungService)
        {
            _buoiHoc_NguoiDungService = buoiHoc_NguoiDungService;
        }

        [HttpPost("nop-bai-tap")]
        public async Task<IActionResult> NopBaiTap([FromBody] ThongTinNopBaiTapViewModel model)
        {
            return await _buoiHoc_NguoiDungService.NopBaiTap(model);
        }

        [HttpPost("nop-capstone")]
        public async Task<IActionResult> NopBaiTapCapstone([FromBody] ThongTinNopBaiTapCapstone model)
        {
            return await _buoiHoc_NguoiDungService.NopBaiTap(model);
        }

        [HttpPost("nop-trac-nghiem")]
        public async Task<IActionResult> NopBaiTapTracNghiem([FromBody] ThongTinNopBaiTapTracNghiemViewModel model)
        {
            return await _buoiHoc_NguoiDungService.NopBaiTap(model);
        }

    }

}