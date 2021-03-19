using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;

namespace ContextViewApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        IConfiguration _configuration;
        IServiceProvider _serviceProvider;
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            _configuration = CreateConfiguration();
            _serviceProvider = RegisterDependencies(_configuration);

            MainWindow = new MainWindow();
            MainWindow.DataContext = _serviceProvider.GetService<IMainWindowViewModel>();


            MainWindow.Show();
        }
        private IConfiguration CreateConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }

        private IServiceProvider RegisterDependencies(IConfiguration configuration)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.Registration(configuration);

            return serviceCollection.BuildServiceProvider();
        }
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.Handled = true;
        }

    }
}
