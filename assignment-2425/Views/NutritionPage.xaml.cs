using assignment_2425.ViewModels;
using assignment_2425.Views;

namespace assignment_2425
{
    public partial class NutritionPage : ContentPage
    {
        public NutritionPage()
        {
            InitializeComponent();
            BindingContext = new NutritionViewModel();
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void OnMenuClicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Options", "Cancel", null, "About page", "Log Out");
            if (action == "About page")
            {
                await Navigation.PushAsync(new AboutPage());
            }
            else if (action == "Log Out")
            {
                bool confirm = await DisplayAlert("Log Out", "Are you sure you want to log out?", "Yes", "No");
                if (confirm)
                {
                    MessagingCenter.Send(this, "ClearProfileData");
                    MessagingCenter.Send(this, "ClearAnalyticsdata");
                    SecureStorage.Remove("firebase_token");
                    SecureStorage.Remove("firebase_uid");
                    await Shell.Current.GoToAsync("//MainPage");
                    await Task.Delay(100);
                    Shell.SetNavBarIsVisible(Shell.Current.CurrentPage, true);
                    NavigationPage.SetHasBackButton(Shell.Current.CurrentPage, true);
                }
            }
        }
    }
}
