using System.Windows;
using DVD26.Services;

namespace DVD26;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        var settingsService = new ToolSettingsService();
        var settings = settingsService.Load();
        if (!settings.HasExistingTools)
        {
            var setupWindow = new ToolSetupWindow(settingsService, settings);
            if (setupWindow.ShowDialog() != true)
            {
                Shutdown();
                return;
            }

            settings = setupWindow.Settings;
        }

        var mainWindow = new MainWindow(settings);
        MainWindow = mainWindow;
        ShutdownMode = ShutdownMode.OnMainWindowClose;
        mainWindow.Show();
    }
}
