using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Quizlet.Interfaces;
using UglyToad.PdfPig;

namespace Quizlet.Services;

public class FileService : IFileService
{
    public async Task<string> ReadFileContentAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found.", filePath);

        var extension = Path.GetExtension(filePath).ToLower();

        return extension switch
        {
            ".txt" => await ReadTxtFileAsync(filePath),
            ".pdf" => ReadPdfFile(filePath),
            ".docx" => ReadDocxFile(filePath),
            _ => throw new NotSupportedException("File type not supported.")
        };
    }

    private static async Task<string> ReadTxtFileAsync(string filePath)
    {
        return await File.ReadAllTextAsync(filePath);
    }

    private static string ReadPdfFile(string filePath)
    {
        var text = new StringBuilder();

        using var stream = File.OpenRead(filePath);
        using var document = PdfDocument.Open(stream);
        foreach (var page in document.GetPages()) text.Append(string.Join(" ", page.GetWords()));

        return text.ToString();
    }

    private static string ReadDocxFile(string filePath)
    {
        using var doc = WordprocessingDocument.Open(filePath, false);
        if (doc.MainDocumentPart?.Document?.Body == null)
            throw new InvalidOperationException("The document structure is invalid or missing.");

        return string.Join(Environment.NewLine, doc.MainDocumentPart.Document.Body.Elements<Paragraph>()
            .Select(p => p.InnerText));
    }
}