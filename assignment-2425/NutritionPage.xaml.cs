using Firebase.Auth;
using Microsoft.Maui.Media; // Import Text-to-Speech
using Microsoft.Maui.Storage; // Required for Secure Storage
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace assignment_2425
{
    public partial class NutritionPage : ContentPage
    {
        private ObservableCollection<FoodItem> FoodLog = new ObservableCollection<FoodItem>();
        private int totalCalories = 0;
        private FirebaseAuthProvider authProvider;

        public NutritionPage()
        {
            InitializeComponent();
            FoodLogCollectionView.ItemsSource = FoodLog;
            UpdateTotalCalories();

            // Initialize FirebaseAuthProvider with my API key
            authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyBVnVmjhhb3tdb8XaIT_31IsEwYjQjN980"));
            
            NavigationPage.SetHasBackButton(this, false);

        }

        private void OnAddCaloriesClicked(object sender, EventArgs e)
        {
            if (int.TryParse(CalorieEntry.Text, out int calories))
            {
                if (calories > 0)
                {
                    FoodLog.Add(new FoodItem { Name = "Manual Entry", Calories = calories });
                    totalCalories += calories;
                    UpdateTotalCalories();
                    CalorieEntry.Text = "";
                }
                else
                {
                    DisplayAlert("Error", "Calories must be greater than zero.", "OK");
                }
            }
            else
            {
                DisplayAlert("Error", "Please enter a valid calorie amount.", "OK");
            }
        }

        private void UpdateTotalCalories()
        {
            TotalCaloriesLabel.Text = $"{totalCalories} kcal";
        }
        private async void OnSaveCaloriesClicked(object sender, EventArgs e)
        {
            string userId = await SecureStorage.GetAsync("firebase_uid"); // Retrieve logged-in user UID
            if (string.IsNullOrEmpty(userId))
            {
                await DisplayAlert("Error", "User ID not found. Please log in again.", "OK");
                return;
            }

            FirestoreService firestoreService = new FirestoreService();
            await firestoreService.SaveCalorieData(userId, totalCalories);

            string message = $"You have consumed {totalCalories} calories today.";
            await TextToSpeech.Default.SpeakAsync(message); // Text-to-Speech

            await DisplayAlert("Success", "Your daily calories have been saved to your profile!", "OK");
        }






        private async void OnResetCaloriesClicked(object sender, EventArgs e)
        {
            bool resetConfirmed = await DisplayAlert("Reset", "Are you sure you want to reset today's calorie count?", "Yes", "No");

            if (resetConfirmed)
            {
                FoodLog.Clear();
                totalCalories = 0;
                UpdateTotalCalories();
            }
        }

        // adding 3 dots menu onto the calorie tracking page 
        private async void OnMenuClicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Options", "Cancel", null, "View Profile", "Log Out");

            if (action == "View Profile")
            {
                await Navigation.PushAsync(new ProfilePage()); // Navigate to Profile Page 
            }
            else if (action == "Log Out")
            {
                bool confirm = await DisplayAlert("Log Out", "Are you sure you want to log out?", "Yes", "No");

                if (confirm)
                {
                    try
                    {
                        // Remove Firebase Token (Ensures the user is logged out)
                        SecureStorage.Remove("firebase_token");

                        // Remove stored user ID
                        SecureStorage.Remove("firebase_uid");

                        // Reset Navigation Stack to Keep Only `MainPage` and `LoginPage`
                        await Shell.Current.GoToAsync("//MainPage");
                        await Task.Delay(100); // Ensure the navigation transition is smooth

                        // Ensure the back button is visible on LoginPage
                        Shell.SetNavBarIsVisible(Shell.Current.CurrentPage, true);
                        NavigationPage.SetHasBackButton(Shell.Current.CurrentPage, true);
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Logout Error", $"Failed to log out: {ex.Message}", "OK");
                    }
                }
            }
        }


    }

    public class FoodItem
    {
        public string Name { get; set; }
        public int Calories { get; set; }
    }
}
