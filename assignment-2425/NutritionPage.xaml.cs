using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;

namespace assignment_2425
{
    public partial class NutritionPage : ContentPage
    {
        private ObservableCollection<FoodItem> FoodLog = new ObservableCollection<FoodItem>();
        private int totalCalories = 0;

        public NutritionPage()
        {
            InitializeComponent();
            FoodLogCollectionView.ItemsSource = FoodLog;
            UpdateTotalCalories();
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

        private void OnResetCaloriesClicked(object sender, EventArgs e)
        {
            bool resetConfirmed = DisplayAlert("Reset", "Are you sure you want to reset today's calorie count?", "Yes", "No").Result;
            if (resetConfirmed)
            {
                FoodLog.Clear();
                totalCalories = 0;
                UpdateTotalCalories();
            }
        }
    }

    public class FoodItem
    {
        public string Name { get; set; }
        public int Calories { get; set; }
    }
}
