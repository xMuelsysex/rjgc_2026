using Avalonia;
using System;
using System.IO;

namespace MyFirstApp;

sealed class Program
{
    private static readonly string LogFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "HospitalWard",
        "app-debug.log");

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath)!);
        File.AppendAllText(LogFilePath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Application starting.{Environment.NewLine}");

        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            File.AppendAllText(
                LogFilePath,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] UnhandledException: {e.ExceptionObject}{Environment.NewLine}");
        };

        try
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            File.AppendAllText(
                LogFilePath,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] Fatal exception in Main: {ex}{Environment.NewLine}");
            throw;
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
