using CommunityToolkit.Mvvm.Input;
using PeakyStart.Domain.Interfaces.Services;
using PeakyStart.Domain.Models;
using System.Collections.ObjectModel;

namespace PeakyTestUI.ViewModels
{
    public partial class CurrencyViewModel : BaseViewModel
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

        // ❌ Убрали: LoadCommand, RefreshCommand, DeleteCommand
        // ✅ Они генерируются автоматически через [RelayCommand]

        public CurrencyViewModel(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await ExecuteAsync(() => _currencyService.RefreshAsync()
                .ContinueWith(_ => _currencyService.GetAllAsync()).Unwrap());
        }

        [RelayCommand]
        private async Task LoadAsync()
        {
            await ExecuteAsync(() => _currencyService.GetAllAsync());
        }

        [RelayCommand]
        private async Task DeleteAsync(Currency? currency)
        {
            if (currency is null) return;

            IsLoading = true;
            ErrorMessage = string.Empty;
            try
            {
                await _currencyService.DeleteAsync(currency.Id);
                Currencies.Remove(currency);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка удаления: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
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