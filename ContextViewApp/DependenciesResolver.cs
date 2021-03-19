using ContextSearchContract;
using ContextSearchImpl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContextViewApp
{
    public static class DependenciesResolver
    {
        public static IServiceCollection Registration(this IServiceCollection serviceProvider, IConfiguration configuration)
        {
            //_logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            //.AddSingleton(_logger)

            serviceProvider
                .AddSingleton<IMainWindowViewModel, MainWindowViewModel>()
                .AddSingleton<IContextSearcher, ContextSearcher>()
                .AddSingleton<IIOProvider, IOProvider>()
                .AddSingleton<ICatilogFactory, CatilogFactory>()
                .AddSingleton<ICatalogWrapper, CatalogWrapper>()
                .AddSingleton<ICatalogToCustomFileConverter, CatalogToCustomFileConverter>();

            return serviceProvider;
        }
    }
}
