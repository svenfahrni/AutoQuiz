using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Quizlet.Interfaces;

namespace Quizlet.Readers
{
    public class DocxFileReader : IFileReaderStrategy
    {
        public bool CanHandle(string fileExtension) => fileExtension.Equals(".docx", StringComparison.OrdinalIgnoreCase);

        public async Task<string> ReadFileContentAsync(string filePath)
        {
            using var doc = WordprocessingDocument.Open(filePath, false);
            if (doc.MainDocumentPart?.Document?.Body == null)
                throw new InvalidOperationException("The document structure is invalid or missing.");

            var content = string.Join(Environment.NewLine, doc.MainDocumentPart.Document.Body.Elements<Paragraph>()
                .Select(p => p.InnerText));

            return await Task.FromResult(content);
        }
    }
}
