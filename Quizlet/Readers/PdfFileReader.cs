using Quizlet.Interfaces;
using System.Text;
using UglyToad.PdfPig;

namespace Quizlet.Readers
{
    public class PdfFileReader : IFileReaderStrategy
    {
        public bool CanHandle(string fileExtension) => fileExtension.Equals(".pdf", StringComparison.OrdinalIgnoreCase);

        public string ReadFileContent(string filePath)
        {
            var text = new StringBuilder();

            using var stream = File.OpenRead(filePath);
            using var document = PdfDocument.Open(stream);
            foreach (var page in document.GetPages()) text.Append(string.Join(" ", page.GetWords()));

            return text.ToString();
        }
    }
}
