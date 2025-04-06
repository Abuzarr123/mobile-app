using assignment_2425.ViewModels;

namespace assignment_2425
{
    public partial class ProfilePage : ContentPage
    {
        private readonly ProfileViewModel vm;

        public ProfilePage()
        {
            InitializeComponent();
            vm = new ProfileViewModel();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await vm.LoadCalorieDataAsync();
        }
    }
}
