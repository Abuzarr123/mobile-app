using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using assignment_2425.Models;
using Microsoft.Maui.Devices;
using Newtonsoft.Json.Linq;

namespace assignment_2425.ViewModels
{
    public partial class BarcodeScanningViewModel : ObservableObject
    {
        [ObservableProperty] 
        private bool isScanned = false;

        public async Task HandleBarcodeDetectedAsync(string barcode)
        {
            if (IsScanned || string.IsNullOrWhiteSpace(barcode))
                return;

            IsScanned = true;

            var foodData = await FetchFoodDataAsync(barcode);

            if (foodData != null)
            {
                try
                {
                     Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(500)); // successfull barcode scan vibrates
                }
                catch (FeatureNotSupportedException)
                {
                    // Vibration not supported on device – safe to ignore
                }

                WeakReferenceMessenger.Default.Send(foodData);
            }

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.Navigation.PopAsync();
            });
        }

        private async Task<FoodScannedMessage?> FetchFoodDataAsync(string barcode) // function to Call to the openfoodfacts api
        {
            try
            {
                using HttpClient client = new();
                var response = await client.GetAsync($"https://world.openfoodfacts.org/api/v0/product/{barcode}.json");

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(json);
                var product = data["product"];

                if (product != null)
                {
                    string name = product.Value<string>("product_name") ?? "Unknown";
                    var nutrients = product["nutriments"];

                    int calories = (int?)nutrients?.Value<float?>("energy-kcal_100g") ?? 0;
                    int protein = (int?)nutrients?.Value<float?>("proteins_100g") ?? 0;
                    int carbs = (int?)nutrients?.Value<float?>("carbohydrates_100g") ?? 0;
                    int fats = (int?)nutrients?.Value<float?>("fat_100g") ?? 0;

                    return new FoodScannedMessage
                    {
                        Barcode = barcode,
                        FoodName = name,
                        Calories = calories,
                        Protein = protein,
                        Carbohydrates = carbs,
                        Fats = fats
                    };
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"API error: {ex.Message}", "OK");
            }

            return null;
        }
    }
}
