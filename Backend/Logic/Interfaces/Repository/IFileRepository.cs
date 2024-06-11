using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces.Repository
{
    public interface IFileRepository
    {
        Task<string> UploadFile(IFormFile file);
    }
}
