using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoloDevApp.Service.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SoloDevApp.Api.Controllers
{
    [Route("api/file")]

    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FileController(IFileService fileService, IHostingEnvironment hostingEnvironment)
        {
            _fileService = fileService;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFile([FromBody] string fileUrl)
        {
            try
            {
                fileUrl = fileUrl.Replace("/", "\\");
                string pathFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var path = @pathFolder + @fileUrl;
               
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return Ok("1");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("file")]
        [RequestSizeLimit(10_000_000_000)]

        public async Task<IActionResult> File()
        {
            try
            {
                IFormFileCollection files = Request.Form.Files;
                List<string> result = await _fileService.UploadFileAsync(files);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("image")]
        [RequestSizeLimit(10_000_000_000)]

        public async Task<IActionResult> Image()
        {
            try
            {
                IFormFileCollection files = Request.Form.Files;
                List<string> result = await _fileService.UploadImageAsync(files);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("cmnd")]

        [RequestSizeLimit(10_000_000_000)]

        public async Task<IActionResult> cmnd([FromForm]IFormCollection files)
        {
            try
            {
               // IFormFileCollection files = Request.Form.Files;
                string result = await _fileService.UploadCmndAsync(files.Files[0]);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("video")]
        public async Task<IActionResult> Video()
        {
            try
            {
                IFormFileCollection files = Request.Form.Files;
                List<string> result = await _fileService.UploadVideoAsync(files);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ftp-video")]

        [RequestSizeLimit(10_000_000_000)]
        public async Task<IActionResult> FtpVideo()
        {
            try
            {
                IFormFileCollection files = Request.Form.Files;
                List<string> result = await _fileService.UploadVideoFTPAsync(files);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-ftp-video")]
        [RequestSizeLimit(10_000_000_000)]

        public async Task<IActionResult> DeleteFtpVideo(string fileName)
        {
            try
            {
                await _fileService.DeleteVideoFTPAsync(fileName);
                return Ok(fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // LẤY URL VIDEO FTP
        [HttpGet("ftp-video/{fileName}")]
        public async Task<IActionResult> FtpVideo(string fileName)
        {
            try
            {
                // Trả về url của video FTP
                string result = await _fileService.GetUrlFTPVideoAsync(fileName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //DOWNLOAD FILE
        [HttpPost("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string pathFile="")
        {
            //string pathFolder = Path.Combine( "files");
            //var coverFolderPath = HttpContext.Server.MapPath(@"~/");
            //var filePath = Path.Combine(coverFolderPath, fileName);
            //pathFile = $@"{this.Request.Scheme}://{this.Request.Host}/files/{pathFile}"  ;
            var fileName = pathFile;
             pathFile = Path.Combine(_hostingEnvironment.WebRootPath,"files/"+pathFile);

            Stream stream = new FileStream( pathFile, FileMode.Open);


            if (stream == null)
                return NotFound(); // returns a NotFoundResult with Status404NotFound response.

            return File(stream, "application/octet-stream",fileName); // return
        }
    }
}