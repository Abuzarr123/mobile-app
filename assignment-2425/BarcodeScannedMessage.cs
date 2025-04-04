// BarcodeScannedMessage.cs
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace assignment_2425
{
    public class BarcodeScannedMessage : ValueChangedMessage<string>
    {
        public BarcodeScannedMessage(string barcode) : base(barcode) { }
    }
}
