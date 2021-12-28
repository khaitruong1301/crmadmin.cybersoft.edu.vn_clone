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

namespace SoloDevApp.Api.Controllers
{
    [Route("api/aparsetoken")]
    [ApiController]


    public class TestParseTokenController : ControllerBase
    {

       

        [HttpGet]
        //[Authorize(Roles = "VIEW_ROLE")]
        public async Task<IActionResult> Get(string tokenstring)
        {
            tokenstring = tokenstring.Replace("Bearer ", "");
            //var stream = Encoding.ASCII.GetBytes("CYBERSOFT2020_SECRET");
            JwtSecurityToken token = new JwtSecurityTokenHandler().ReadJwtToken(tokenstring);
            var email = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

            return new ResponseEntity(StatusCodeConstants.OK, email);
        }

    }
}