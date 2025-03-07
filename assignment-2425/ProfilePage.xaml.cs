using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System.Collections.ObjectModel;
using System;

namespace assignment_2425
{
    public partial class ProfilePage : ContentPage
    {
        public ObservableCollection<CalorieRecord> CalorieRecords { get; set; }

        public ProfilePage()
        {
            InitializeComponent();
            CalorieRecords = new ObservableCollection<CalorieRecord>();

            LoadCalorieData();
            CalorieLogCollectionView.ItemsSource = CalorieRecords;
        }

        private async void LoadCalorieData()
        {
            var storedData = await SecureStorage.GetAsync("calorie_log");

            if (!string.IsNullOrEmpty(storedData))
            {
                CalorieRecords.Clear();

                string[] entries = storedData.Split(';');

                foreach (var entry in entries)
                {
                    string[] parts = entry.Split(',');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int calories))
                    {
                        var calorieRecord = new CalorieRecord
                        {
                            Date = parts[0],
                            Calories = calories.ToString() + " kcal"
                        };
                        CalorieRecords.Add(calorieRecord);
                    }
                }
            }
        }
    }

    public class CalorieRecord
    {
        public string Date { get; set; }

        // Ensuring "kcal" is always included correctly
        public string CaloriesFormatted => Calories + " kcal";

        public string Calories {  get; set; }
    }

}
