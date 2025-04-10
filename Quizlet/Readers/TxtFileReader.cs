using Quizlet.Interfaces;

namespace Quizlet.Readers
{
    public class TxtFileReader : IFileReaderStrategy
    {
        public bool CanHandle(string fileExtension) => fileExtension.Equals(".txt", StringComparison.OrdinalIgnoreCase);

        public async Task<string> ReadFileContentAsync(string filePath)
        {
            return await File.ReadAllTextAsync(filePath);
        }
    }
}
