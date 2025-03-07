namespace assignment_2425
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            Shell.SetNavBarIsVisible(this, true);

            // Hides the back button as this is not needed on the home page
            NavigationPage.SetHasBackButton(this, false);



        }



    }

}
