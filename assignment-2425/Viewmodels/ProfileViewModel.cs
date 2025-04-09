using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Storage;
using assignment_2425.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace assignment_2425.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly FirestoreService firestoreService;

        public ObservableCollection<CalorieRecord> CalorieRecords { get; } = new();

        public ProfileViewModel()
        {
            firestoreService = new FirestoreService();

            
            MessagingCenter.Subscribe<NutritionPage>(this, "ClearProfileData", (sender) =>
            {
                CalorieRecords.Clear();
            });
        }

        public async Task LoadCalorieDataAsync() //function that lods the users calorie data from firestore database
        {
            string userId = await SecureStorage.GetAsync("firebase_uid");

            if (string.IsNullOrEmpty(userId)) return;

            var fetched = await firestoreService.GetCalorieData(userId);
            CalorieRecords.Clear();
            foreach (var item in fetched)
                CalorieRecords.Add(item);
        }
        [RelayCommand]
        private async Task DeleteCalorie(CalorieRecord item) //function so that user can delete calorie data
        {
            if (item == null) return;

            bool confirm = await Application.Current.MainPage.DisplayAlert("Delete Entry",
                $"Are you sure you want to delete {item.FoodName} from {item.Date}?", "Yes", "No");

            if (!confirm) return;

            CalorieRecords.Remove(item);

            string userId = await SecureStorage.GetAsync("firebase_uid");
            if (!string.IsNullOrEmpty(userId))
            {
                await firestoreService.DeleteCalorieData(userId, item); 
            }
        }
        [RelayCommand]
        private async Task EditCalorie(CalorieRecord item)
        {
            if (item == null) return;

            // Prompt user for updated values
            string newFoodName = await Application.Current.MainPage.DisplayPromptAsync("Edit Food Name", "Enter new food name:", initialValue: item.FoodName);
            string newCalories = await Application.Current.MainPage.DisplayPromptAsync("Edit Calories", "Enter new calorie value:", initialValue: item.Calories.Replace(" kcal", ""), keyboard: Keyboard.Numeric);
            string newProtein = await Application.Current.MainPage.DisplayPromptAsync("Edit Protein", "Enter new protein amount (g):", initialValue: item.protein.ToString(), keyboard: Keyboard.Numeric);
            string newCarbs = await Application.Current.MainPage.DisplayPromptAsync("Edit Carbohydrates", "Enter new carbohydrate amount (g):", initialValue: item.carbohydrates.ToString(), keyboard: Keyboard.Numeric);
            string newFats = await Application.Current.MainPage.DisplayPromptAsync("Edit Fats", "Enter new fat amount (g):", initialValue: item.fats.ToString(), keyboard: Keyboard.Numeric);

            // Validation
            if (string.IsNullOrWhiteSpace(newFoodName) ||
                !int.TryParse(newCalories, out int updatedCalories) ||
                !int.TryParse(newProtein, out int updatedProtein) ||
                !int.TryParse(newCarbs, out int updatedCarbs) ||
                !int.TryParse(newFats, out int updatedFats))
            {
                await Application.Current.MainPage.DisplayAlert("Invalid Input", "Please enter valid numbers for all nutritional values.", "OK");
                return;
            }

            // Remove old entry
            CalorieRecords.Remove(item);

            // Create new updated item
            var updatedItem = new CalorieRecord
            {
                Date = item.Date,
                Calories = $"{updatedCalories} kcal",
                FoodName = newFoodName,
                protein = updatedProtein,
                carbohydrates = updatedCarbs,
                fats = updatedFats
            };

            CalorieRecords.Add(updatedItem);

            // Update Firebase
            string userId = await SecureStorage.GetAsync("firebase_uid");

            if (!string.IsNullOrEmpty(userId))
            {
                await firestoreService.DeleteCalorieData(userId, item);
                await firestoreService.SaveCalorieData(userId, updatedCalories, newFoodName, updatedProtein, updatedCarbs, updatedFats);
            }
        }
    }
}
