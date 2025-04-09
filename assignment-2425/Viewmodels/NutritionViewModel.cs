using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using assignment_2425.Models;
using System.Linq;
using System.Windows.Input;


namespace assignment_2425.ViewModels
{
    public partial class NutritionViewModel : ObservableObject
    {
        private readonly FirestoreService firestoreService;

        [ObservableProperty] private string foodName;
        [ObservableProperty] private string calories;
        [ObservableProperty] private string protein;
        [ObservableProperty] private string carbs;
        [ObservableProperty] private string fats;

        [ObservableProperty] private int totalCalories;

        public ObservableCollection<FoodItem> FoodLog { get; } = new();

        public NutritionViewModel()
        {
            firestoreService = new FirestoreService();

            // listens for message from the barcode scan
            WeakReferenceMessenger.Default.Register<FoodScannedMessage>(this, (r, msg) =>
            {
                FoodName = msg.FoodName;
                Calories = msg.Calories.ToString();
                Protein = msg.Protein.ToString();
                Carbs = msg.Carbohydrates.ToString();
                Fats = msg.Fats.ToString();
            });
        }

        [RelayCommand]
        private async void AddCalories() // function to add calories to food log
        {
            if (string.IsNullOrWhiteSpace(FoodName) ||
                string.IsNullOrWhiteSpace(Calories) ||
                string.IsNullOrWhiteSpace(Protein) ||
                string.IsNullOrWhiteSpace(Carbs) ||
                string.IsNullOrWhiteSpace(Fats))
            {
                await ShowAlert("Missing Information", "Please fill in all fields before adding.");
                return;
            }

            if (int.TryParse(Calories, out int cal) &&
                int.TryParse(Protein, out int pro) &&
                int.TryParse(Carbs, out int carb) &&
                int.TryParse(Fats, out int fat))
            {
                FoodLog.Add(new FoodItem
                {
                    Name = FoodName,
                    Calories = cal,
                    Protein = pro,
                    Carbohydrates = carb,
                    Fats = fat
                });

                TotalCalories += cal;

                FoodName = Calories = Protein = Carbs = Fats = string.Empty;
            }
        }

        [RelayCommand]
        public async Task SaveCaloriesAsync() // function to save calories to firestore database
        {
            if (!FoodLog.Any()) 
            {
                await ShowAlert("No data", "Please fill out the values before saving");
                return;
            }
            

            string userId = await SecureStorage.GetAsync("firebase_uid");
            if (string.IsNullOrEmpty(userId)) return;

            var lastItem = FoodLog.Last();
            await firestoreService.SaveCalorieData(userId, lastItem.Calories, lastItem.Name, lastItem.Protein, lastItem.Carbohydrates, lastItem.Fats);

            bool isTtsEnabled = Preferences.Get("TTS_Enabled", true);
            if (isTtsEnabled)
            {
                await TextToSpeech.Default.SpeakAsync($"You have consumed {TotalCalories} calories today.");
            }
                await ShowAlert("Saved", "Your data has been saved to your profile.");
        }

        [RelayCommand]
        public void ResetCalories() // resets total calories
        {
            FoodLog.Clear();
            TotalCalories = 0;
        }

        [RelayCommand]
        public async Task ScanBarcodeAsync() // Navigates to barcode scanning page
        {
            await Shell.Current.Navigation.PushAsync(new BarcodeScanning());
        }
        private async Task ShowAlert(string title, string message)
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
                Application.Current.MainPage.DisplayAlert(title, message, "OK"));
        }

    }
}
