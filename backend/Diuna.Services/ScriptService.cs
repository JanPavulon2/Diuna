using System.Diagnostics;

namespace Diuna.Services;

public interface IScriptService
{
    string RunScript(string scriptPath);
}

public class ScriptService : IScriptService
{
    public string RunScript(string scriptPath)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "python3", // or "python" depending on your environment
                Arguments = scriptPath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception($"Script failed with exit code {process.ExitCode}: {error}");
        }

        return output;
    }
}
