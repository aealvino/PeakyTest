using PeakyStart.Domain.Interfaces.Services;
using PeakyStart.Domain.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;

namespace PeakyTestUI.ViewModels
{
    public class CurrencyViewModel : BaseViewModel
    {
        private readonly ICurrencyService _currencyService;

        public ObservableCollection<Currency> Currencies { get; } = new();

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetField(ref _isLoading, value);
        }

        private string _errorMessage = string.Empty;

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetField(ref _errorMessage, value);
        }

        public ICommand LoadCommand { get; }
        public ICommand RefreshCommand { get; }

        public CurrencyViewModel(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
            LoadCommand = new RelayCommand(LoadAsync);
            RefreshCommand = new RelayCommand(RefreshAsync);
        }

        private async Task RefreshAsync()
        {
            await ExecuteAsync(() => _currencyService.RefreshAsync()
                .ContinueWith(_ => _currencyService.GetAllAsync()).Unwrap());
        }

        private async Task LoadAsync()
        {
            await ExecuteAsync(() => _currencyService.GetAllAsync());
        }

        private async Task ExecuteAsync(Func<Task<IEnumerable<Currency>>> action)
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            try
            {
                var result = await action();
                Currencies.Clear();
                foreach (var c in result)
                    Currencies.Add(c);
            }
            catch (HttpRequestException ex)
            {
                ErrorMessage = $"Нет соединения: {ex.Message}";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}