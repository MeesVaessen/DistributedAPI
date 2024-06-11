using FluentFTP;
using Logic.Interfaces.Repository;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Dal.Repository
{
    public class FileRepository : IFileRepository
    {
        IAsyncFtpClient _ftpClient;
        public FileRepository(IAsyncFtpClient ftpClient)
        {
            _ftpClient = ftpClient;
            _ftpClient.Host = "192.168.0.2";
            _ftpClient.Credentials = new NetworkCredential("Apiuser", "P@ssword");
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            // Generate a unique file name to prevent overwriting
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            try
            {
                Console.WriteLine(_ftpClient.Host);
                Console.WriteLine(_ftpClient.Credentials?.UserName + " and " + _ftpClient.Credentials?.Password);
                await _ftpClient.Connect();
                Console.WriteLine("Connected");

                // Upload the file
                using (var fileStream = file.OpenReadStream())
                {
                    var result = await _ftpClient.UploadStream(fileStream, uniqueFileName);
                    Console.WriteLine(result);
                }
                Console.WriteLine(uniqueFileName);

                // Return the file path (in this case, the FTP URL)
                return uniqueFileName;
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions
                throw new Exception($"Error uploading file via FTP: {ex.Message}");
            }
            finally
            {
                await _ftpClient.Disconnect();
            }
        }
    }
}
