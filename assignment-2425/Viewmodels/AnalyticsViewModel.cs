using assignment_2425.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace assignment_2425.ViewModels
{
    public partial class AnalyticsViewModel : ObservableObject
    {
        private readonly FirestoreService firestoreService;
        public ObservableCollection<CalorieChartData> CalorieData { get; } = new();

        public AnalyticsViewModel()
        {
            firestoreService = new FirestoreService();

            LoadFirebaseDataAsync();

            MessagingCenter.Subscribe<NutritionPage>(this, "ClearAnalyticsdata", (sender) =>
            {
                CalorieData.Clear();
            });
        }

        private async void LoadFirebaseDataAsync() //loads the user data from firebase 
        {
            string userId = await SecureStorage.GetAsync("firebase_uid");
            if (string.IsNullOrEmpty(userId)) return;

            var records = await firestoreService.GetCalorieData(userId);
            CalorieData.Clear();

            foreach (var record in records)
            {
                if (record.Calories?.Contains("kcal") == true &&
                    int.TryParse(record.Calories.Replace(" kcal", ""), out int calValue))
                {
                    CalorieData.Add(new CalorieChartData
                    {
                        Date = record.Date,
                        Calories = calValue
                    });
                }
            }
        }
        public async Task RefreshDataAsync() // refreshes the data 
        {
            string userId = await SecureStorage.GetAsync("firebase_uid");
            if (string.IsNullOrEmpty(userId)) return;

            var records = await firestoreService.GetCalorieData(userId);
            CalorieData.Clear();

            foreach (var record in records)
            {
                if (record.Calories?.Contains("kcal") == true &&
                    int.TryParse(record.Calories.Replace(" kcal", ""), out int calValue))
                {
                    CalorieData.Add(new CalorieChartData
                    {
                        Date = record.Date,
                        Calories = calValue
                    });
                }
            }
        }


    }
}
