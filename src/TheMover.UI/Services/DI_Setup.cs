using Microsoft.Extensions.DependencyInjection;
using TheMover.UI.ViewModels;
using TheMover.UI.Views;

namespace TheMover.UI.Services {

    // ReSharper disable once InconsistentNaming -> Its ok here, this class is very on of a kind
    internal static class DI_Setup {
        public static ServiceProvider GetServiceProvider() {
            ServiceCollection services = [];
            services.AddScoped<MainWindowViewModel>();
            services.AddSingleton<FileIconService>();
            services.AddSingleton<FrontendMovablePackageBuilder>();

            return services.BuildServiceProvider();
        }
    }
}
