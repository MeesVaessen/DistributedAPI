using Api.Model;
using Logic.Interfaces.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IFileManager _fileManager;
        private readonly WebSocketHandler _websocketHandler;
        public FileController(IConfiguration configuration, IFileManager fileManager, WebSocketHandler webSocketHandler)
        {
            _configuration = configuration;
            _fileManager = fileManager;
            _websocketHandler = webSocketHandler;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload(IFormFile file, string wsToken)
        {
            if (string.IsNullOrEmpty(wsToken) || !_websocketHandler.TokenToWebSocketMap.ContainsKey(wsToken))
            {
                return BadRequest("Invalid or missing token.");
            }

            if (file != null || file.Length != 0)
            {
                var filename = await _fileManager.UploadFile(file);

                var result = await _websocketHandler.SendMessageAsync("File_Uploaded", wsToken, filename);
                Console.WriteLine(result.ToString());
            }
           
            return Ok("File uploaded successfully.");
        }


        [HttpPost]
        [Route("uploadHash")]
        public async Task<IActionResult> UploadHash(UploadModel model, string wsToken)
        {
            //Hash code  
            await Task.CompletedTask;
            return Ok("hash uploaded successfully.");
        }

        [HttpPost]
        [Route("sendMessage")]
        public async Task<IActionResult> SendWebsocketMessage(string message, string wsToken)
        {
            if (message == null || message == "")
                return BadRequest("No message given.");

            await _websocketHandler.SendMessageAsync("Message", wsToken, message);
            return Ok("Message send successfully.");
        }
    }
}