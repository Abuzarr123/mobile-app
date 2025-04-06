using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Storage;
using assignment_2425.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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

        public async Task LoadCalorieDataAsync()
        {
            string userId = await SecureStorage.GetAsync("firebase_uid");

            if (string.IsNullOrEmpty(userId)) return;

            var fetched = await firestoreService.GetCalorieData(userId);
            CalorieRecords.Clear();
            foreach (var item in fetched)
                CalorieRecords.Add(item);
        }
    }
}
