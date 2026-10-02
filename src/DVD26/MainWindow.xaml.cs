using System.Windows;
using DVD26.ViewModels;
using DVD26.Services;
using Microsoft.Win32;

namespace DVD26;

public partial class MainWindow : Window
{
    private MainViewModel ViewModel => (MainViewModel)DataContext;

    public MainWindow(ToolSettings settings)
    {
        InitializeComponent();
        DataContext = new MainViewModel(settings);
    }

    private void BrowseSource_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog { Title = "Select a DVD drive or VIDEO_TS folder" };
        if (dialog.ShowDialog(this) == true) ViewModel.SourcePath = dialog.FolderName;
    }

    private void BrowseOutput_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog { Title = "Select an archive destination" };
        if (dialog.ShowDialog(this) == true) ViewModel.OutputPath = dialog.FolderName;
    }

    private void Scan_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Status = "Scanner wiring is the next milestone; preview data remains visible.";
    }
}
