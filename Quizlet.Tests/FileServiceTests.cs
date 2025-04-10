using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.DependencyInjection;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Quizlet.Interfaces;
using Quizlet.Readers;
using Quizlet.Services;

namespace Quizlet.Tests;

public class FileServiceTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly FileService _fileService;

    public FileServiceTests()
    {
        var serviceCollection = new ServiceCollection();

        // Registrierung der Strategien
        serviceCollection.AddTransient<IFileReaderStrategy, TxtFileReader>();
        serviceCollection.AddTransient<IFileReaderStrategy, PdfFileReader>();
        serviceCollection.AddTransient<IFileReaderStrategy, DocxFileReader>();

        // Registrierung des FileService
        serviceCollection.AddTransient<FileService>();

        _serviceProvider = serviceCollection.BuildServiceProvider();
        _fileService = _serviceProvider.GetRequiredService<FileService>();
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


    [Fact]
    public async Task ReadFileContentAsync_ShouldReturnContent_ForPdfFile()
    {
        // Arrange
        var filePath = "testfile.pdf";
        var expectedContent = "Dies ist ein Testinhalt.";
        CreatePdfFile(filePath, expectedContent);

        // Act
        var content = await _fileService.ReadFileContentAsync(filePath);

        // Assert
        Assert.Equal(expectedContent, content);

        // Cleanup
        File.Delete(filePath);
    }

    [Fact]
    public async Task ReadFileContentAsync_ShouldThrowNotSupportedException_WhenFileTypeIsNotSupported()
    {
        // Arrange
        var filePath = "testfile.invalidfileformat";
        await File.WriteAllTextAsync(filePath, "Test content");

        // Act & Assert
        await Assert.ThrowsAsync<NotSupportedException>(() => _fileService.ReadFileContentAsync(filePath));

        // Cleanup
        File.Delete(filePath);
    }

    [Fact]
    public async Task ReadFileContentAsync_ShouldThrowInvalidOperationException_WhenDocxFileStructureIsInvalid()
    {
        // Arrange
        var filePath = "invalidfile.docx";
        CreateDocxFileWithInvalidStructure(filePath);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _fileService.ReadFileContentAsync(filePath));

        // Cleanup
        File.Delete(filePath);
    }


    private static void CreateDocxFile(string filePath, string content)
    {
        using var doc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document);
        var mainPart = doc.AddMainDocumentPart();
        mainPart.Document = new Document();
        var body = mainPart.Document.AppendChild(new Body());
        var para = body.AppendChild(new Paragraph());
        var run = para.AppendChild(new Run());
        run.AppendChild(new Text(content));
    }

    private static void CreateDocxFileWithInvalidStructure(string filePath)
    {
        using var doc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document);
        var mainPart = doc.AddMainDocumentPart();
        mainPart.Document = new Document();
        // Intentionally not adding a body to create an invalid structure
    }

    private static void CreatePdfFile(string filePath, string content)
    {
        var document = new PdfDocument();
        document.Info.Title = "Test Document";
        var page = document.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        var font = new XFont("Verdana", 12);
        gfx.DrawString(content, font, XBrushes.Black,
            new XPoint(20, 20),
            XStringFormats.TopLeft);
        document.Save(filePath);
        document.Close();
    }
}