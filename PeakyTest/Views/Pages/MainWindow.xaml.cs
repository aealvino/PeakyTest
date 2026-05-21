using PeakyTestUI.Views;
using PeakyTestUI.Views.Pages;
using System.Windows;

namespace PeakyTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GoToMainPage(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MainPageView());
        }

        private void GoToCurrenciesPage(object sender, RoutedEventArgs e)
        {
            var page = new CurrenciesPageView
            {
                DataContext = App.CurrencyViewModel
            };
            MainFrame.Navigate(page);
        }

        private void GoToAddCurrencyPage(object sender, RoutedEventArgs e)
        {
            var page = new AddCurrencyPageView
            {
                DataContext = App.AddCurrencyViewModel
            };
            MainFrame.Navigate(page);
        }
    }
}