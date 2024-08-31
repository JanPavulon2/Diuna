namespace Diuna.Services.Helpers;

public static class PathHelper
{
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