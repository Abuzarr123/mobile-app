using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace assignment_2425
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> FontSizeOptions { get; } = new ObservableCollection<string>
        {
            "Small", "Medium", "Large", "Extra Large"
        };

        private string selectedFontSize;
        public string SelectedFontSize
        {
            get => selectedFontSize;
            set
            {
                if (selectedFontSize != value)
                {
                    selectedFontSize = value;
                    OnPropertyChanged();
                    UpdateFontSizeValue();
                }
            }
        }

        private double fontSizeValue = 14;
        public double FontSizeValue
        {
            get => fontSizeValue;
            set
            {
                if (fontSizeValue != value)
                {
                    fontSizeValue = value;
                    OnPropertyChanged();
                }
            }
        }

        private void UpdateFontSizeValue()
        {
            FontSizeValue = SelectedFontSize switch
            {
                "Small" => 12,
                "Medium" => 14,
                "Large" => 18,
                "Extra Large" => 22,
                _ => 14
            };
            Application.Current.Resources["AppFontSize"] = FontSizeValue;

        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
