using Microsoft.AspNetCore.Mvc;
using Quizlet.Interfaces;
using Quizlet.Models;
namespace Quizlet.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        private readonly ICardDeckGenerationService _cardDeckGenerationService;
        private readonly IFileService _fileService;

        public FileController(IFileService fileService, ICardDeckGenerationService cardDeckGenerationService)
        {
            _fileService = fileService;
            _cardDeckGenerationService = cardDeckGenerationService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile formFile)
        {
            // Validate the content
            if (formFile == null || formFile.Length == 0) 
            {
                Console.WriteLine("No file uploaded.");
                return BadRequest(new { message = "No file uploaded." });
            }
            Console.WriteLine(formFile.FileName);

            // Validate the file type
            var allowedExtensions = new[] { ".txt", ".pdf", ".docx" };
            var fileExtension = Path.GetExtension(formFile.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension)) 
            {
                Console.WriteLine("Invalid file type. Only .txt, .pdf, and .docx files are allowed.");
                return BadRequest(new { message = "Invalid file type. Only .txt, .pdf, and .docx files are allowed." });
            }

            // Save the file to a temporary location
            var safeFileName = Path.GetFileName(formFile.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}_{safeFileName}";
            var tempFilePath = Path.Combine(Path.GetTempPath(), uniqueFileName);
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            // Read the file content
            string content;
            try
            {
                content = await _fileService.ReadFileContentAsync(tempFilePath);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            finally
            {
                // Clean up the temporary file
                if (System.IO.File.Exists(tempFilePath))
                    System.IO.File.Delete(tempFilePath);
            }
            // Return the content
            CardDeck cardDeck = await _cardDeckGenerationService.GenerateCardsFromTextAsync(content);
            return Ok(cardDeck);
        }
    }
}
