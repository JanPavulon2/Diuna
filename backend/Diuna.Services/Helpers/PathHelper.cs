namespace Diuna.Services.Helpers;

public static class PathHelper
{

    public static string GetConfigFilePath(string fileName)
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var configPath = Path.Combine(baseDirectory, "config", fileName);
        
        if(!File.Exists(configPath))
        {
            throw new FileNotFoundException($"Configuration file '{fileName}' not found at '{configPath}'.");
        }

        return configPath;
    }

    public static string GetSolutionDirectory()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var directoryInfo = new DirectoryInfo(baseDirectory);

        while (directoryInfo != null && !directoryInfo.GetFiles("*.sln").Any())
        {
            directoryInfo = directoryInfo.Parent;
        }

        return directoryInfo?.FullName ?? throw new Exception("Solution directory not found.");
    }
}