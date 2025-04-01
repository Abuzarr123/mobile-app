using Google.Cloud.Firestore;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace assignment_2425
{
    public partial class ProfilePage : ContentPage
    {
        public ObservableCollection<CalorieRecord> CalorieRecords { get; set; }
        private FirestoreService firestoreService;

        public ProfilePage()
        {
            InitializeComponent();
            this.BindingContext = this; // Ensure data binding works

            firestoreService = new FirestoreService();
            CalorieRecords = new ObservableCollection<CalorieRecord>();

            //LoadCalorieData();
            CalorieLogCollectionView.ItemsSource = CalorieRecords;
                MessagingCenter.Subscribe<NutritionPage>(this, "ClearProfileData", sender =>
                {
                    CalorieRecords.Clear();
                });
        }

        private async Task LoadCalorieData()
        {
            string userId = await SecureStorage.GetAsync("firebase_uid");

            if (string.IsNullOrEmpty(userId))
            {
                await DisplayAlert("Error", "User ID not found. Please log in again.", "OK");
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            var fetchedData = await firestoreService.GetCalorieData(userId);

            CalorieRecords.Clear();

            foreach (var record in fetchedData)
            {
                CalorieRecords.Add(record);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadCalorieData(); //Refreshes each time the tab for profile is clicked
        }


    }

    public class CalorieRecord
    {
        public string Date { get; set; }
        public string Calories { get; set; }
    }
}
