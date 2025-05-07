namespace Quizlet.Interfaces;

public interface IFileService
{
    Task<string> ReadFileContentAsync(string filePath);
}