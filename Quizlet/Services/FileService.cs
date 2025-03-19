using System.Text;
using DocumentFormat.OpenXml.Packaging;
using Quizlet.Interfaces;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace Quizlet.Services
{
    public class FileService : IFileService
    {
        public async Task<string> ReadFileContentAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found.", filePath);

            string extension = Path.GetExtension(filePath).ToLower();

            return extension switch
            {
                ".txt" => await ReadTxtFileAsync(filePath),
                ".pdf" => ReadPdfFile(filePath),
                ".docx" => ReadDocxFile(filePath),
                _ => throw new NotSupportedException("File type not supported.")
            };
        }

        private async Task<string> ReadTxtFileAsync(string filePath)
        {
            return await File.ReadAllTextAsync(filePath);
        }

        private string ReadPdfFile(string filePath)
        {
            StringBuilder text = new StringBuilder();

            using (
                var stream = File.OpenRead(filePath))
            using (PdfDocument document = PdfDocument.Open(stream))
            {
                foreach(Page page in document.GetPages())
                {
                    text.Append(string.Join(" ", page.GetWords()));
                }
            }

            return text.ToString();
        }

        private string ReadDocxFile(string filePath)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, false))
            {
                return string.Join(Environment.NewLine, doc.MainDocumentPart.Document.Body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>()
                    .Select(p => p.InnerText));
            }
        }
    }
}