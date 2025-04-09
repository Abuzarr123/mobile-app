using System.IO;
using Microsoft.Maui.Storage;
using ZXing.Net.Maui.Controls;

namespace assignment_2425
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseBarcodeReader() // register Zxing
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });



            CopyFirebaseCredentials(); // Copy Firestore Credentials on App Start

            return builder.Build();
        }

        private static void CopyFirebaseCredentials()// firebasecredentials.json file being added
        {   
            string sourceFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Raw", "firebase_credentials.json");
            string destinationFile = Path.Combine(FileSystem.AppDataDirectory, "firebase_credentials.json");

            if (!File.Exists(destinationFile))
            {
                try
                {
                    using Stream fileStream = FileSystem.OpenAppPackageFileAsync("firebase_credentials.json").Result;
                    using FileStream destStream = File.Create(destinationFile);
                    fileStream.CopyTo(destStream);
                    Console.WriteLine("Firebase credentials copied successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error copying Firebase credentials: {ex.Message}");
                }
            }
        }

    }
}
