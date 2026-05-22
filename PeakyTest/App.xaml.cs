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
        public static AddCurrencyViewModel AddCurrencyViewModel { get; private set; } = null!;
        public static MainPageViewModel MainPageViewModel { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var db = new DatabaseContext();
            db.Database.EnsureCreated();

            var settingsService = new SettingsService(db);
            MainPageViewModel = new MainPageViewModel(settingsService); 
            settingsService.SaveLastSession();

            var httpClient = new HttpClient();

            var httpRepo = new CurrencyApiRepository(httpClient);
            var dbRepo = new CurrencyDbRepository(db);
            var jsonRepo = new CurrencyJsonRepository();

            ICurrencyService service = new CurrencyService(httpRepo, dbRepo);

            CurrencyViewModel = new CurrencyViewModel(service, dbRepo, jsonRepo);
            AddCurrencyViewModel = new AddCurrencyViewModel(service);
            new MainWindow().Show();
        }
    }

}
