using PeakyStart.Domain.Interfaces.Services;
using PeakyStart.Domain.Models;
using System.Collections.ObjectModel;
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

        public CurrencyViewModel(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
            LoadCommand = new RelayCommand(LoadAsync);
        }
        private async Task LoadAsync()
        {
            IsLoading = true;

            try
            {
                var result = await _currencyService.GetAllAsync();

                Currencies.Clear();
                foreach (var c in result)
                    Currencies.Add(c);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка загрузки: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}