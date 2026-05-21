using PeakyStart.Domain.Interfaces.Repositories;
using PeakyStart.Domain.Interfaces.Services;
using PeakyStart.Infrastructure.Repositories;
using PeakyStart.Infrastructure.Services;
using PeakyTestUI.ViewModels;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;

namespace PeakyTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static CurrencyViewModel CurrencyViewModel { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var httpClient = new HttpClient();
            ICurrencyRepository repository = new CurrencyRepository(httpClient);
            ICurrencyService service = new CurrencyService(repository);
            CurrencyViewModel = new CurrencyViewModel(service);

            new MainWindow().Show();
        }
    }

}
