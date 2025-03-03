using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace assignment_2425
{
    public class MainPageViewModel
    {
        public ICommand SignUpCommand { get; }

        public MainPageViewModel()
        {
            SignUpCommand = new Command(OnSignUpClicked);
        }

        private async void OnSignUpClicked()
        {
            // Navigate to the LoginPage
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}