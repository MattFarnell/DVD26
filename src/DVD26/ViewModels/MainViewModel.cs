using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DVD26.Models;
using DVD26.Services;

namespace DVD26.ViewModels;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private string _sourcePath = "D:\\";
    private string _outputPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
    private string _toolStatus = "Checking FFmpeg tools…";
    private string _status = "Choose an unprotected DVD source to begin.";
    private ArchiveFormat _selectedFormat = ArchiveFormat.LosslessMkv;
    private DvdTitle? _selectedTitle;

    public MainViewModel(ToolSettings toolSettings)
    {
        Formats = new Dictionary<ArchiveFormat, string>
        {
            [ArchiveFormat.LosslessMkv] = "Lossless MKV (stream copy)",
            [ArchiveFormat.H264Mp4] = "MP4 (H.264)",
            [ArchiveFormat.VideoTsFolder] = "VIDEO_TS folder copy",
            [ArchiveFormat.IsoImage] = "ISO image copy"
        };
        LoadDesignPreview();
        _ = CheckToolsAsync(toolSettings);
    }

    public IReadOnlyDictionary<ArchiveFormat, string> Formats { get; }
    public ObservableCollection<DvdTitle> Titles { get; } = [];
    public string SourcePath { get => _sourcePath; set => Set(ref _sourcePath, value); }
    public string OutputPath { get => _outputPath; set => Set(ref _outputPath, value); }
    public string ToolStatus { get => _toolStatus; private set => Set(ref _toolStatus, value); }
    public string Status { get => _status; set => Set(ref _status, value); }
    public ArchiveFormat SelectedFormat { get => _selectedFormat; set => Set(ref _selectedFormat, value); }
    public DvdTitle? SelectedTitle { get => _selectedTitle; set => Set(ref _selectedTitle, value); }

    private async Task CheckToolsAsync(ToolSettings toolSettings)
    {
        var unsupportedComponent = UnsupportedComponentDetector.FindInApplicationDirectories();
        if (unsupportedComponent is not null)
        {
            ToolStatus = "Unsupported component detected: remove libdvdcss.dll";
            Status = "DVD26 does not load copy-protection circumvention libraries.";
            return;
        }

        var ffmpeg = await ExternalToolLocator.CheckAsync(toolSettings.FfmpegPath);
        var ffprobe = await ExternalToolLocator.CheckAsync(toolSettings.FfprobePath);
        ToolStatus = ffmpeg.IsAvailable && ffprobe.IsAvailable
            ? "FFmpeg and ffprobe are ready"
            : "Setup needed: select valid ffmpeg.exe and ffprobe.exe files";
    }

    private void LoadDesignPreview()
    {
        var feature = new DvdTitle { Number = 1, Name = "Main feature (suggested)", Duration = new(1, 48, 22) };
        feature.AudioTracks.Add(new() { Name = "English — AC-3 5.1", Details = "48 kHz · 448 kb/s", IsSelected = true });
        feature.AudioTracks.Add(new() { Name = "Director commentary — AC-3 2.0", Details = "48 kHz · 192 kb/s" });
        feature.SubtitleTracks.Add(new() { Name = "English", Details = "DVD bitmap subtitle · soft", IsSelected = true });
        feature.SubtitleTracks.Add(new() { Name = "Spanish", Details = "DVD bitmap subtitle · soft" });
        Titles.Add(feature);
        Titles.Add(new() { Number = 2, Name = "Bonus feature", Duration = new(0, 12, 5) });
        Titles.Add(new() { Number = 3, Name = "Menu", Duration = new(0, 1, 14) });
        SelectedTitle = feature;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        PropertyChanged?.Invoke(this, new(propertyName));
    }
}
