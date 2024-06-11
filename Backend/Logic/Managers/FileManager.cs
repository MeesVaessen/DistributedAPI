using Logic.Interfaces.Manager;
using Logic.Interfaces.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Managers
{
    public class FileManager : IFileManager
    {
        IFileRepository fileRepository;
        public FileManager(IFileRepository _fileRepository)
        {
            fileRepository = _fileRepository;
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            return await fileRepository.UploadFile(file);
        }
    }
}
