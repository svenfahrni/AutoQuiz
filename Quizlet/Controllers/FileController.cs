using Microsoft.AspNetCore.Mvc;
using Quizlet.Interfaces;
using Quizlet.Models;

namespace Quizlet.Controllers;

[ApiController]
[Route("api/files")]
public class FileController(IFileService fileService, ICardDeckGenerationService cardDeckGenerationService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile? formFile)
    {
        // Validate the content
        if (formFile == null || formFile.Length == 0)
        {
            Console.WriteLine("No file uploaded.");
            return BadRequest(new { message = "No file uploaded." });
        }

        // Validate the file type
        var allowedExtensions = new[] { ".txt", ".pdf", ".docx" };
        var fileExtension = Path.GetExtension(formFile.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(fileExtension))
        {
            Console.WriteLine("Invalid file type. Only .txt, .pdf, and .docx files are allowed.");
            return BadRequest(new { message = "Invalid file type. Only .txt, .pdf, and .docx files are allowed." });
        }

        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var tempFilePath = Path.Combine(Path.GetTempPath(), uniqueFileName);

        try
        {
            await using (var stream = new FileStream(tempFilePath, FileMode.CreateNew))
            {
                await formFile.CopyToAsync(stream);
            }

            CardDeck cardDeck;
            // Read the file content
            try
            {
                string content = await fileService.ReadFileContentAsync(tempFilePath);
                cardDeck = await cardDeckGenerationService.GenerateCardsFromTextAsync(content);
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
            return Ok(cardDeck);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"File processing failed: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
        finally
        {
            if (System.IO.File.Exists(tempFilePath)) System.IO.File.Delete(tempFilePath);
        }
    }
}