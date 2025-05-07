using Quizlet.Interfaces;

namespace Quizlet.Readers
{
    public class TxtFileReader : IFileReaderStrategy
    {
        public bool CanHandle(string fileExtension) => fileExtension.Equals(".txt", StringComparison.OrdinalIgnoreCase);

        public string ReadFileContent(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
