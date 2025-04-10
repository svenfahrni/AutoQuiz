using Quizlet.Interfaces;

namespace Quizlet.Services;

public class FileService : IFileService
{
    private readonly IEnumerable<IFileReaderStrategy> _fileReaderStrategies;

    public FileService(IEnumerable<IFileReaderStrategy> fileReaderStrategies)
    {
        _fileReaderStrategies = fileReaderStrategies;
    }

    public async Task<string> ReadFileContentAsync(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found.", filePath);

        var extension = Path.GetExtension(filePath).ToLower();
        var strategy = _fileReaderStrategies.FirstOrDefault(s => s.CanHandle(extension));

        if (strategy == null)
            throw new NotSupportedException($"Extension '{extension}' isn't supported.");

        return await strategy.ReadFileContentAsync(filePath);
    }
}