using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoloDevApp.Service.Constants;
using SoloDevApp.Service.Services;
using SoloDevApp.Service.ViewModels;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SoloDevApp.Service.Utilities;
using System;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/aparsetoken")]
    [ApiController]


    public class TestParseTokenController : ControllerBase
    {

        [HttpGet]
        //[Authorize(Roles = "VIEW_ROLE")]
        public async Task<IActionResult> Get([FromHeader] string Token)
        {
            string userId = FuncUtilities.GetUserIdFromHeaderToken(Token);

            return new ResponseEntity(StatusCodeConstants.OK, userId);
        }


        [HttpGet("test-string-date")]
        //[Authorize(Roles = "VIEW_ROLE")]
        public async Task<IActionResult> testHamNgayThang()
        {

            //Demo việc cập nhật ngày hiện tại vào db
            //Lấy ngày hiện tại
            DateTime NgayHienTai = FuncUtilities.GetDateTimeCurrent();

            //dữ liệu dạng json nên phải chuyển về string để lưu dưới db
            string ngayHienTaiString = FuncUtilities.ConvertDateToString(NgayHienTai);

            //Demo việc lấy dữ liệu ngày ra để kiểm tra coi cách ngày hiện tại bao nhiêu ngày

            int khoangCachNgay = FuncUtilities.TinhKhoangCachNgay(FuncUtilities.ConvertStringToDate(ngayHienTaiString));
           

            return new ResponseEntity(StatusCodeConstants.OK, khoangCachNgay);
        }


    }
}