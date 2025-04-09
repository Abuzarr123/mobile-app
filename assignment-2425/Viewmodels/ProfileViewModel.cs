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
    }
}
