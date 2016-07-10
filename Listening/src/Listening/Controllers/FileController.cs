using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using WebListening.Exceptions;

namespace WebListening.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private string _path;

        private readonly IConfiguration _configuration;

        public FileController()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            _configuration = builder.Build();

#if DEBUG
            _path = "audio";
#else
            _path = Directory.GetCurrentDirectory() + _configuration["FileStorage:AudioPath"];
#endif
        }

        [HttpPut("{id}")]
        public JsonResult PutAudioFile(string id, IList<IFormFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var filesInFolder = Directory.GetFiles(_path);
                        filesInFolder = filesInFolder
                            .Select(x => x.Split('/').Last()).ToArray();
                        if (!filesInFolder.Contains(fileName))
                            file.SaveAs(Path.Combine(_path, fileName));
                        else
                            throw new FileUploadException($"File with name {fileName} is already exists.");
                    }
                }

                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json("");
            }
            catch (FileUploadException ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid operation");
            }
        }

        [HttpDelete("{name}")]
        public JsonResult DeleteFile(string name)
        {
            try
            {
                var fullPath = $"{_path}/{name}";

                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
                else
                    throw new FileUploadException($"Can`t remove file with name {name} due to his inexistence.");

                return Json("");
            }
            catch (FileUploadException ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid operation");
            }
        }
    }
}
