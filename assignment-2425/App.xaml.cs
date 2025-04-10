using Microsoft.Maui.Networking;

namespace assignment_2425
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            double savedFontSize = Preferences.Get("AppFontSize", 16); // save the appfont size to be 16 
            Application.Current.Resources["AppFontSize"] = savedFontSize;

            MainPage = new AppShell();

            Connectivity.Current.ConnectivityChanged += Connectivity_ConnectivityChanged;

        }

        private bool isAlertDisplayed = false;


        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;

            if (access != NetworkAccess.Internet)
            {
                if (!isAlertDisplayed)
                {
                    isAlertDisplayed = true;
                    await MainPage.DisplayAlert("Connection Lost", "Please reconnect to the internet.", "OK");
                    isAlertDisplayed = false;
                }
            }
        }
    }
}
