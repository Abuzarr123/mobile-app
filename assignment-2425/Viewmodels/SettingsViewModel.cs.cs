using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace assignment_2425
{
    public class SettingsViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<string> FontSizeOptions { get; } = new ObservableCollection<string>
        {
            "Small", "Medium", "Large", "Extra Large" //sets the names of the different font types 
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
                _ => 14 // sets a range of values based on different font sizes and base font being 14
            };
            Application.Current.Resources["AppFontSize"] = FontSizeValue;

        }
        private bool isTextToSpeechEnabled;

        public bool IsTextToSpeechEnabled
        {
            get => isTextToSpeechEnabled;
            set
            {
                if (isTextToSpeechEnabled != value)
                {
                    isTextToSpeechEnabled = value;
                    Preferences.Set("TTS_Enabled", value); //TTS
                    OnPropertyChanged();
                }
            }
        }
        public SettingsViewModel()
        {
            IsTextToSpeechEnabled = Preferences.Get("TTS_Enabled", true);
        }

        public event PropertyChangedEventHandler PropertyChanged;   
        void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
