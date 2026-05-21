using PeakyStart.Domain.Interfaces.Services;
using PeakyStart.Infrastructure.Persistence;
using PeakyStart.Infrastructure.Repositories;
using PeakyStart.Infrastructure.Services;
using PeakyTestUI.ViewModels;
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

            var db = new DatabaseContext();
            db.Database.EnsureCreated();

            var httpClient = new HttpClient();

            var httpRepo = new CurrencyApiRepository(httpClient);
            var localRepo = new CurrencyDbRepository(db);

            ICurrencyService service = new CurrencyService(httpRepo, localRepo);

            CurrencyViewModel = new CurrencyViewModel(service);
            new MainWindow().Show();
        }
    }

}
