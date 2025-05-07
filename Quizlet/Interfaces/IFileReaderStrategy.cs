namespace Quizlet.Interfaces
{
    public interface IFileReaderStrategy
    {
        bool CanHandle(string fileExtension);
        string ReadFileContent(string filePath);
    }
}
