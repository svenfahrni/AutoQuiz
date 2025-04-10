namespace Quizlet.Interfaces
{
    public interface IFileReaderStrategy
    {
        bool CanHandle(string fileExtension);
        Task<string> ReadFileContentAsync(string filePath);
    }
}
