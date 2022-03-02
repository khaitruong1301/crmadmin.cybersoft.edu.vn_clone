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

            DateTime current = FuncUtilities.GetDateTimeCurrent();

            string ngayString = FuncUtilities.ConvertDateToString(current);

           
            //DateTime dateTime = FuncUtilities.ConvertStringToDateTime(current);
            int khoangCachNgayTest = FuncUtilities.TinhKhoangCachNgay(FuncUtilities.ConvertStringToDateTime(ngayString));
            



            return new ResponseEntity(StatusCodeConstants.OK, khoangCachNgayTest);
        }


    }
}