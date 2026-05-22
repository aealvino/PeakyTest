using CommunityToolkit.Mvvm.Input;
using PeakyStart.Domain.Interfaces.Services;
using PeakyStart.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PeakyTestUI.ViewModels
{
    public partial class AddCurrencyViewModel : BaseViewModel
    {
        private readonly ICurrencyService _currencyService;

        private string _id = string.Empty;
        public string Id
        {
            get => _id;
            set => SetField(ref _id, value);
        }

        private string _charCode = string.Empty;

        [Required(ErrorMessage = "Код обязателен")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Код должен содержать ровно 3 буквы")]
        public string CharCode
        {
            get => _charCode;
            set
            {
                SetProperty(ref _charCode, value);
                ValidateProperty(value, nameof(CharCode));
            }
        }

        private string _numCode = string.Empty;

        [Required(ErrorMessage = "Цифровой код обязателен")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "Цифровой код должен содержать ровно 3 цифры")]
        public string NumCode
        {
            get => _numCode;
            set
            {
                SetProperty(ref _numCode, value);
                ValidateProperty(value, nameof(NumCode));
            }
        }

        private string _name = string.Empty;

        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Название должно быть от 2 до 50 символов")]
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                ValidateProperty(value, nameof(Name));
            }
        }

        private string _nominal = string.Empty;

        [Required(ErrorMessage = "Номинал обязателен")]
        [RegularExpression(@"^\d{1,10}$", ErrorMessage = "Введите число от 1 до 10 цифр")]
        public string Nominal
        {
            get => _nominal;
            set
            {
                SetProperty(ref _nominal, value);
                ValidateProperty(value, nameof(Nominal));
            }
        }

        private string _value = string.Empty;

        [Required(ErrorMessage = "Курс обязателен")]
        [RegularExpression(@"^\d{1,10}([.,]\d{1,4})?$", ErrorMessage = "Введите число от 1 до 10 цифр")]
        public string Value
        {
            get => _value;
            set
            {
                SetProperty(ref _value, value);
                ValidateProperty(value, nameof(Value));
            }
        }

        private string _previous = string.Empty;

        [Required(ErrorMessage = "Вчерашний курс обязателен")]
        [RegularExpression(@"^\d{1,10}([.,]\d{1,4})?$", ErrorMessage = "Введите число от 1 до 10 цифр")]
        public string Previous
        {
            get => _previous;
            set
            {
                SetProperty(ref _previous, value);
                ValidateProperty(value, nameof(Previous));
            }
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

        public event Action? OnAdded;

        public AddCurrencyViewModel(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [RelayCommand]
        private async Task AddAsync()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;

            ValidateAllProperties();
            if (HasErrors) return;

            IsLoading = true;
            try
            {
                var currency = new Currency
                {
                    Id = string.IsNullOrWhiteSpace(Id) ? Guid.NewGuid().ToString() : Id,
                    NumCode = NumCode,
                    CharCode = CharCode,
                    Name = Name,
                    Nominal = int.Parse(Nominal),
                    Value = double.Parse(Value, CultureInfo.InvariantCulture),
                    Previous = double.Parse(Previous, CultureInfo.InvariantCulture)
                };

                await _currencyService.AddAsync(currency);

                SuccessMessage = $"Валюта «{Name}» успешно добавлена.";
                OnAdded?.Invoke();
                ClearFields();
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

        [RelayCommand]
        private void ClearFields()
        {
            Id = string.Empty;
            CharCode = string.Empty;
            NumCode = string.Empty;
            Name = string.Empty;
            Nominal = string.Empty;
            Value = string.Empty;
            Previous = string.Empty;

            ErrorMessage = string.Empty;

            ClearErrors();
        }
    }
}