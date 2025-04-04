using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls;
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

    private void OnBarcodeDetected(object sender, BarcodeDetectionEventArgs e)
    {
        if (isScanned || e.Results.Length == 0)
            return;

        isScanned = true;

        var barcode = e.Results.FirstOrDefault()?.Value;

        if (!string.IsNullOrEmpty(barcode))
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                // Send the scanned barcode using the Messenger
                WeakReferenceMessenger.Default.Send(new BarcodeScannedMessage(barcode));

                // Go back safely
                if (Navigation.NavigationStack.Count > 1)
                {
                    await Navigation.PopAsync();
                }
            });
        }
    }
}
