using PeakyStart.Domain.Interfaces.Services;

namespace PeakyTestUI.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public string LastSessionText { get; }

        public MainPageViewModel(ISettingsService settingsService)
        {
            var last = settingsService.LoadLastSession();

            LastSessionText = last.HasValue
                ? $"Последний вход: {last.Value:dd.MM.yyyy HH:mm:ss}"
                : "Первый запуск приложения";
        }
    }
}