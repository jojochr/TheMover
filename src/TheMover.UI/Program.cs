﻿using Avalonia;
using System;

namespace TheMover.UI;

public static class Program {
    /// <summary>
    /// Initialization code. Don't use any Avalonia, third-party APIs or any
    /// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    /// yet and stuff might break.
    /// </summary>
    [STAThread]
    public static void Main(string[] args) {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    /// <summary>Avalonia configuration, don't remove; also used by visual designer.</summary>
    private static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
                                                             .UsePlatformDetect()
                                                             .WithInterFont()
                                                             .LogToTrace();
}