namespace assignment_2425
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            double savedFontSize = Preferences.Get("AppFontSize", 14); // save the appfont size to be 14 
            Application.Current.Resources["AppFontSize"] = savedFontSize;

            MainPage = new AppShell();
        }
    }
}
