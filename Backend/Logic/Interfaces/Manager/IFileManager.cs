using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces.Manager
{
    public interface IFileManager
    {
        Task<string> UploadFile(IFormFile file);
    }
}
