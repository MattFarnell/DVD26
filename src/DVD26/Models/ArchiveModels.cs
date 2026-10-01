using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DVD26.Models;

public enum ArchiveFormat
{
    LosslessMkv,
    H264Mp4,
    VideoTsFolder,
    IsoImage
}

public sealed class TrackOption : INotifyPropertyChanged
{
    private bool _isSelected;
    public required string Name { get; init; }
    public required string Details { get; init; }
    public bool IsSelected
    {
        get => _isSelected;
        set { _isSelected = value; PropertyChanged?.Invoke(this, new(nameof(IsSelected))); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}

public sealed class DvdTitle
{
    public required int Number { get; init; }
    public required string Name { get; init; }
    public required TimeSpan Duration { get; init; }
    public string DurationText => Duration.ToString(@"hh\:mm\:ss");
    public ObservableCollection<TrackOption> AudioTracks { get; } = [];
    public ObservableCollection<TrackOption> SubtitleTracks { get; } = [];
}
