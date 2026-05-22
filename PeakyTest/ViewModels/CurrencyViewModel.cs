using CommunityToolkit.Mvvm.Input;
using PeakyStart.Domain.Interfaces.Repositories;
using PeakyStart.Domain.Interfaces.Services;
using PeakyStart.Domain.Models;
using PeakyStart.Domain.Models.Enums;
using System.Collections.ObjectModel;

namespace PeakyTestUI.ViewModels
{
    public partial class CurrencyViewModel : BaseViewModel
    {
        private readonly ICurrencyService _currencyService;
        private readonly ICurrencyDbRepository _dbRepository;
        private readonly ICurrencyJsonRepository _jsonRepository;

        public ObservableCollection<Currency> Currencies { get; } = new();
        public List<StorageTypeEnum> StorageTypes { get; } = Enum.GetValues<StorageTypeEnum>().ToList();

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

        private StorageTypeEnum _selectedStorageType;

        public StorageTypeEnum SelectedStorageType
        {
            get => _selectedStorageType;
            set
            {
                SetField(ref _selectedStorageType, value);
                SwitchStorage(value);
            }
        }

        private void SwitchStorage(StorageTypeEnum storageType)
        {
            if (storageType == StorageTypeEnum.SQLite)
                _currencyService.SetRepository(_dbRepository);
            else
            _currencyService.SetRepository(_jsonRepository);
        }

        public CurrencyViewModel(ICurrencyService currencyService, ICurrencyDbRepository dbRepository, ICurrencyJsonRepository jsonRepository)
        {
            _currencyService = currencyService;
            _dbRepository = dbRepository;
            _jsonRepository = jsonRepository;
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
            if (currency is null)
            {
                ErrorMessage = "Валюта не выбрана";
                return;
            }

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