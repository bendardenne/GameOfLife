using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using GameOfLife.Models;
using GameOfLife.ViewModels;
using GameOfLife.Views;
using Splat;
using Splat.NLog;

namespace GameOfLife;

public partial class App : Application
{
    public override void Initialize()
    {
        Locator.CurrentMutable.UseNLogWithWrappingFullLogger();
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            ExpressionObserver.DataValidators.RemoveAll(x => x is DataAnnotationsValidationPlugin);

            // Create model and register as a singleton. 
            var universe = new Universe(50, 50);
            universe.Reseed();

            Locator.CurrentMutable.RegisterConstant(universe, typeof(Universe));

            var timeService = new TimeService(new TimeSpan(0, 0, 0, 0, 100));
            timeService += universe.PassTime;
            timeService.Start();

            Locator.CurrentMutable.RegisterConstant(timeService, typeof(ITimeService));

            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}