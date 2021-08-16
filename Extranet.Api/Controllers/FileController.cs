using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Extranet.Api.Controllers
{
    [Route( "[controller]" )]
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;

        public FileController( IWebHostEnvironment appEnvironment )
        {
            _appEnvironment = appEnvironment;
        }

        [HttpPost, Route( "upload" )]
        public async Task<string> UploadFile( IFormFile uploadedFile )
        {
            if ( uploadedFile != null )
            {
                string projectPath = System.IO.Directory.GetCurrentDirectory();
                string fileName = Guid.NewGuid().ToString() + "." + uploadedFile.FileName.Split( '.' )[ 1 ];
                string path = "/Files/" + fileName;
                using ( var fileStream = new FileStream( projectPath + path, FileMode.Create ) )
                {
                    await uploadedFile.CopyToAsync( fileStream );
                }

                return "file/download/" + fileName;
            }
            return "";
        }

        [HttpGet, Route( "download/{filename}" )]
        public async Task<IActionResult> DownloadFile( string filename )
        {
            string filePath = System.IO.Directory.GetCurrentDirectory() + "/Files/" + filename;
            var bytes = await System.IO.File.ReadAllBytesAsync( filePath );
            return File( bytes, "image/" + filename.Split( '.' )[ 1 ] );
        }

    }
}
