namespace DVD26.Services;

/// <summary>
/// Protects the application's non-circumvention boundary. This type only checks
/// for unsupported files; it never loads, invokes, or inspects their contents.
/// </summary>
public static class UnsupportedComponentDetector
{
    private static readonly string[] UnsupportedFileNames = ["libdvdcss.dll"];

    public static string? FindInApplicationDirectories()
    {
        var directories = new[]
        {
            AppContext.BaseDirectory,
            Environment.CurrentDirectory
        };

        foreach (var directory in directories.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            foreach (var fileName in UnsupportedFileNames)
            {
                var candidate = Path.Combine(directory, fileName);
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }
        }

        return null;
    }
}
