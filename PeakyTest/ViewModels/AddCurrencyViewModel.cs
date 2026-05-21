using PeakyStart.Domain.Interfaces.Services;
using PeakyStart.Domain.Models;
using System.Windows.Input;

namespace PeakyTestUI.ViewModels
{
    public class AddCurrencyViewModel : BaseViewModel
    {
        private ICurrencyService _currencyService;

        private string id = string.Empty;
        public string Id
        {
            get => id;
            set => SetField(ref id, value);
        }

        private string _charCode = string.Empty;
        public string CharCode
        {
            get => _charCode;
            set => SetField(ref _charCode, value);
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetField(ref _name, value);
        }

        private string _nominal = string.Empty;
        public string Nominal
        {
            get => _nominal;
            set => SetField(ref _nominal, value);
        }

        private string _value = string.Empty;
        public string Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }

        private string _previous = string.Empty;
        public string Previous
        {
            get => _previous;
            set => SetField(ref _previous, value);
        }

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

        private string _successMessage = string.Empty;
        public string SuccessMessage
        {
            get => _successMessage;
            set => SetField(ref _successMessage, value);
        }

        public ICommand AddCommand { get; }
        public ICommand ClearCommand { get; }
        
        public event Action? OnAdded;

        public AddCurrencyViewModel(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
            AddCommand = new RelayCommand(AddAsync);
            ClearCommand = new RelayCommand(ClearFields);
        }

        private async Task AddAsync()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(CharCode) || string.IsNullOrWhiteSpace(Name))
            {
                ErrorMessage = "Поля «Код» и «Валюта» обязательны.";
                return;
            }

            if (!double.TryParse(Value, out var parsedValue))
            {
                ErrorMessage = "Некорректное значение курса.";
                return;
            }

            if (!double.TryParse(Previous, out var parsedPrevious))
            {
                ErrorMessage = "Некорректное значение «Вчера».";
                return;
            }

            if (!int.TryParse(Nominal, out var parsedNominal))
            {
                ErrorMessage = "Некорректный номинал.";
                return;
            }

            IsLoading = true;
            try
            {
                var currency = new Currency
                {
                    Id = string.IsNullOrWhiteSpace(Id) ? Guid.NewGuid().ToString() : Id,
                    CharCode = CharCode,
                    Name = Name,
                    Nominal = parsedNominal,
                    Value = parsedValue,
                    Previous = parsedPrevious
                };

                await _currencyService.AddAsync(currency);

                SuccessMessage = $"Валюта «{Name}» успешно добавлена.";
                OnAdded?.Invoke();
                await ClearFields();
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

        private Task ClearFields()
        {
            Id = string.Empty;
            CharCode = string.Empty;
            Name = string.Empty;
            Nominal = string.Empty;
            Value = string.Empty;
            Previous = string.Empty;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            return Task.CompletedTask;
        }
    }
}
