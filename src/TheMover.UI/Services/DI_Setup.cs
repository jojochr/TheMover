using System;
using Microsoft.Extensions.DependencyInjection;
using TheMover.Domain.Logging;
using TheMover.Infrastructure.Provider;
using TheMover.Infrastructure.Services;
using TheMover.UI.ViewModels;
using TheMover.UI.ViewModels.Design;
using TheMover.UI.Views;

namespace TheMover.UI.Services {

    // ReSharper disable once InconsistentNaming -> Its ok here, this class is very on of a kind
    internal static class DI_Setup {
        public static ServiceProvider GetServiceProvider() {
            ServiceCollection services = [];

            // From Domain
#if RELEASE
            Logger logger = new(useConsole: false, useLogDirectory: true);
#else
            Logger logger = new(useConsole: true, useLogDirectory: true);
#endif
            services.AddSingleton(logger);

            // From Infrastructure Project
            PackageProvider.GetInitializedPackageProvider(logger).Match(success: packageProvider => services.AddSingleton(packageProvider),
                                                                        failure: _ => logger.Log(new LogMessage("Could not create PackageProvider on initialization",
                                                                                                                LogMessageSeverity.Panic, ErrorKey.Panic)));
            services.AddSingleton<RepositoryWatcher>();

            // From UI
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<PackageOperationsViewModel>();
            services.AddTransient<DesignPackageOperationsViewModel>();
            services.AddSingleton<FileIconService>();
            services.AddSingleton<FrontendMovablePackageBuilder>();

            return services.BuildServiceProvider();
        }
    }
}
