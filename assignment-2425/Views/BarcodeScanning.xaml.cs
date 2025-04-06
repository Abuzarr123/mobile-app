using assignment_2425.ViewModels;

namespace assignment_2425
{
    public partial class BarcodeScanning : ContentPage
    {
        private readonly BarcodeScanningViewModel viewModel;

        public BarcodeScanning()
        {
            InitializeComponent();
            viewModel = new BarcodeScanningViewModel();
            BindingContext = viewModel;

            cameraView.Options = new ZXing.Net.Maui.BarcodeReaderOptions
            {
                Formats = ZXing.Net.Maui.BarcodeFormats.All,
                AutoRotate = true,
                Multiple = false
            };
        }

        private async void OnBarcodeDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
        {
            var barcode = e.Results.FirstOrDefault()?.Value;

            if (!string.IsNullOrWhiteSpace(barcode))
            {
                await viewModel.HandleBarcodeDetectedAsync(barcode);
            }
        }
    }
}
