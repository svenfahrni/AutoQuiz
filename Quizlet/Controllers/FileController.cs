using Microsoft.AspNetCore.Mvc;
using Quizlet.Interfaces;

namespace Quizlet.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("read")]
        public async Task<IActionResult> ReadFile([FromQuery] string filePath)
        {
            try
            {
                var content = await _fileService.ReadFileContentAsync(filePath);
                return Ok(new { content });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
