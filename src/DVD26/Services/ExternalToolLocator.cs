using System.Diagnostics;

namespace DVD26.Services;

public sealed record ToolStatus(bool IsAvailable, string Message);

public static class ExternalToolLocator
{
    public static async Task<ToolStatus> CheckAsync(string executable, CancellationToken cancellationToken = default)
    {
        try
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo(executable, "-version")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            process.Start();
            await process.WaitForExitAsync(cancellationToken);
            var firstLine = (await process.StandardOutput.ReadToEndAsync(cancellationToken))
                .Split('\n', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            return process.ExitCode == 0
                ? new(true, firstLine?.Trim() ?? $"{executable} is available")
                : new(false, $"{executable} exited with code {process.ExitCode}");
        }
        catch (Exception ex) when (ex is System.ComponentModel.Win32Exception or FileNotFoundException)
        {
            return new(false, $"{executable} was not found on PATH");
        }
    }
}
