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
    [Route("api/baihocnew")]
    [ApiController]
    //[ApiKeyAuth]
    public class BaiHocNewController : ControllerBase
    {
        private IBaiHocNewService _baiHocNewService;
        

        public BaiHocNewController(IBaiHocNewService baiHocNewService)
        {
            _baiHocNewService = baiHocNewService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await _baiHocNewService.GetAllAsync();
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(int page, int size, string keywords = "")
        {
            return await _baiHocNewService.GetPagingAsync(page, size, keywords);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return await _baiHocNewService.GetSingleByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BaiHoc_TaiLieu_Link_TracNghiemViewModel model)
        {
            return await _baiHocNewService.InsertAsync(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BaiHoc_TaiLieu_Link_TracNghiemViewModel model)
        {
            return await _baiHocNewService.UpdateAsync(id, model);
        }


        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] List<dynamic> Ids)
        {
            return await _baiHocNewService.DeleteByIdAsync(Ids);
        }


    }

}