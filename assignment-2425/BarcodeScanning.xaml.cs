using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls;
using System.Text.Json;
using ZXing.Net.Maui;

namespace assignment_2425;

public partial class BarcodeScanning : ContentPage
{
    private bool isScanned = false;

    public BarcodeScanning()
    {
        InitializeComponent();

        cameraView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.All,
            AutoRotate = true,
            Multiple = false
        };
    }


    private async void OnBarcodeDetected(object sender, BarcodeDetectionEventArgs e)
    {
        if (isScanned || e.Results.Length == 0)
            return;

        isScanned = true;

        var barcode = e.Results.FirstOrDefault()?.Value;

        if (!string.IsNullOrEmpty(barcode))
        {
            var foodData = await FetchFoodDataAsync(barcode);

            if (foodData != null)
            {
                WeakReferenceMessenger.Default.Send(foodData);
            }

            if (Navigation.NavigationStack.Count > 1)
            {
                await Navigation.PopAsync();
            }
        }
    }
    private async Task<FoodScannedMessage?> FetchFoodDataAsync(string barcode)
    {
        try
        {
            using HttpClient client = new HttpClient();
            var response = await client.GetAsync($"https://world.openfoodfacts.org/api/v0/product/{barcode}.json");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var data = Newtonsoft.Json.Linq.JObject.Parse(json);
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
            await DisplayAlert("Error", $"API error: {ex.Message}", "OK");
        }

        return null;
    }





    public class FoodScannedMessage
    {
        public string Barcode { get; set; }
        public string FoodName { get; set; }
        public int Calories { get; set; }
        public int Protein { get; set; }
        public int Carbohydrates { get; set; }
        public int Fats { get; set; }
    }


}
