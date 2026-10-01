using System.IO;
using System.Windows;
using DVD26.Services;
using Microsoft.Win32;

namespace DVD26;

public partial class ToolSetupWindow : Window
{
    private readonly ToolSettingsService _settingsService;
    public ToolSettings Settings { get; private set; }

    public ToolSetupWindow(ToolSettingsService settingsService, ToolSettings settings)
    {
        InitializeComponent();
        _settingsService = settingsService;
        Settings = settings;
        FfmpegPathBox.Text = settings.FfmpegPath;
        FfprobePathBox.Text = settings.FfprobePath;
    }

    private void SelectFfmpeg_Click(object sender, RoutedEventArgs e) =>
        SelectExecutable("ffmpeg.exe", FfmpegPathBox);

    private void SelectFfprobe_Click(object sender, RoutedEventArgs e) =>
        SelectExecutable("ffprobe.exe", FfprobePathBox);

    private void SelectExecutable(string expectedName, System.Windows.Controls.TextBox target)
    {
        var dialog = new OpenFileDialog
        {
            Title = $"Select {expectedName}",
            Filter = $"{expectedName}|{expectedName}",
            CheckFileExists = true,
            Multiselect = false
        };
        if (dialog.ShowDialog(this) == true) target.Text = dialog.FileName;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        if (!IsExpectedExecutable(FfmpegPathBox.Text, "ffmpeg.exe") ||
            !IsExpectedExecutable(FfprobePathBox.Text, "ffprobe.exe"))
        {
            ValidationText.Text = "Select valid ffmpeg.exe and ffprobe.exe files before continuing.";
            return;
        }

        Settings = new(FfmpegPathBox.Text, FfprobePathBox.Text);
        _settingsService.Save(Settings);
        DialogResult = true;
    }

    private static bool IsExpectedExecutable(string path, string name) =>
        File.Exists(path) && string.Equals(Path.GetFileName(path), name, StringComparison.OrdinalIgnoreCase);

    private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
