using assignment_2425.ViewModels;

namespace assignment_2425.Views
{
    public partial class AnalyticsPage : ContentPage
    {
        private readonly AnalyticsViewModel vm;

        public AnalyticsPage()
        {
            InitializeComponent();
            vm = new AnalyticsViewModel();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await vm.RefreshDataAsync();
        }
    }
}
