using DocumentFormat.OpenXml.Packaging;
using Quizlet.Interfaces;
using Quizlet.Services;

namespace Quizlet.Tests
{
    public class FileServiceTests
    {
        private readonly IFileService _fileService;

        public FileServiceTests()
        {
            _fileService = new FileService();
        }

        [Fact]
        public async Task ReadFileContentAsync_ShouldThrowArgumentException_WhenFilePathIsNullOrEmpty()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _fileService.ReadFileContentAsync(null));
            await Assert.ThrowsAsync<ArgumentException>(() => _fileService.ReadFileContentAsync(string.Empty));
        }

        [Fact]
        public async Task ReadFileContentAsync_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
        {
            // Arrange
            var nonExistentFilePath = "nonexistentfile.txt";

            // Act & Assert
            await Assert.ThrowsAsync<FileNotFoundException>(() => _fileService.ReadFileContentAsync(nonExistentFilePath));
        }

        [Fact]
        public async Task ReadFileContentAsync_ShouldReturnContent_ForTxtFile()
        {
            // Arrange
            var filePath = "testfile.txt";
            var expectedContent = "Dies ist ein Testinhalt.";
            await File.WriteAllTextAsync(filePath, expectedContent);

            // Act
            var content = await _fileService.ReadFileContentAsync(filePath);

            // Assert
            Assert.Equal(expectedContent, content);

            // Cleanup
            File.Delete(filePath);
        }

        [Fact]
        public async Task ReadFileContentAsync_ShouldReturnContent_ForDocxFile()
        {
            // Arrange
            var filePath = "testfile.docx";
            var expectedContent = "Dies ist ein Testinhalt.";
            CreateDocxFile(filePath, expectedContent);

            // Act
            var content = await _fileService.ReadFileContentAsync(filePath);

            // Assert
            Assert.Equal(expectedContent, content);

            // Cleanup
            File.Delete(filePath);
        }

        private void CreateDocxFile(string filePath, string content)
        {
            using (var doc = WordprocessingDocument.Create(filePath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                var mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                var body = mainPart.Document.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Body());
                var para = body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph());
                var run = para.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                run.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Text(content));
            }
        }
    }
}
