using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using MyFirstApp.Services;
using MyFirstApp.ViewModels;
using MyFirstApp.Views;

namespace MyFirstApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            var loginViewModel = new LoginViewModel();
            var registerViewModel = new RegisterViewModel();
            var loginWindow = new Window
            {
                Width = 560,
                Height = 720,
                CanResize = false,
                Title = "登录 / 注册",
                Content = new TabControl
                {
                    ItemsSource = new object[]
                    {
                        new TabItem
                        {
                            Header = "登录",
                            Content = new LoginView { DataContext = loginViewModel }
                        },
                        new TabItem
                        {
                            Header = "注册",
                            Content = new RegisterView { DataContext = registerViewModel }
                        }
                    }
                }
            };

            loginWindow.Closed += (_, _) =>
            {
                if (!loginViewModel.LoginSucceeded)
                {
                    desktop.Shutdown();
                }
            };

            loginViewModel.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(LoginViewModel.LoginSucceeded) && loginViewModel.LoginSucceeded)
                {
                    desktop.MainWindow = new MainWindow
                    {
                        DataContext = new MainWindowViewModel(UserSessionService.Instance),
                    };
                    desktop.MainWindow.Show();
                    loginWindow.Close();
                }
            };

            desktop.MainWindow = loginWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
