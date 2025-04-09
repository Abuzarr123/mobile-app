using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Media;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace assignment_2425
{
    public partial class SettingsViewModel : INotifyPropertyChanged
    {
        private ImageSource profileImage;
        public ImageSource ProfileImage
        {
            get => profileImage;
            set
            {
                if (profileImage != value)
                {
                    profileImage = value;
                    OnPropertyChanged();
                }
            }
        }

        [RelayCommand]
        public async Task ChangeProfilePictureAsync()
        {
            string action = await Shell.Current.DisplayActionSheet("Choose profile picture", "Cancel", null, "Take Photo", "Choose from Gallery");

            if (action == "Take Photo")
            {
                await CapturePhotoAsync();
            }
            else if (action == "Choose from Gallery")
            {
                await PickFromGalleryAsync();
            }
        }

        private async Task PickFromGalleryAsync()
        {
            try
            {
                FileResult result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select a profile picture",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    Preferences.Set("ProfileImagePath", result.FullPath);
                    ProfileImage = ImageSource.FromFile(result.FullPath);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Could not pick image: {ex.Message}", "OK");
            }
        }

        private async Task CapturePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();

                if (photo != null)
                {
                    var newFile = Path.Combine(FileSystem.AppDataDirectory, photo.FileName);

                    using var stream = await photo.OpenReadAsync();
                    using var newStream = File.OpenWrite(newFile);
                    await stream.CopyToAsync(newStream);

                    Preferences.Set("ProfileImagePath", newFile);
                    ProfileImage = ImageSource.FromFile(newFile);
                }
            }
            catch (PermissionException)
            {
                await Shell.Current.DisplayAlert("Error", "Camera permission denied.", "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error", $"Could not take photo: {ex.Message}", "OK");
            }
        }

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

        private double fontSizeValue = 16;
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
                _ => 16 // sets a range of values based on different font sizes and base font being 16
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
                    Preferences.Set("TTS_Enabled", value);
                    OnPropertyChanged();
                }
            }
        }

        private bool isDarkModeEnabled;
        public bool IsDarkModeEnabled
        {
            get => isDarkModeEnabled;
            set
            {
                if (isDarkModeEnabled != value)
                {
                    isDarkModeEnabled = value;
                    Preferences.Set("DarkMode_Enabled", value);
                    ApplyTheme(value);
                    OnPropertyChanged();
                }
            }
        }

        private bool isHapticEnabled;
        public bool IsHapticEnabled
        {
            get => isHapticEnabled;
            set
            {
                if (isHapticEnabled != value)
                {
                    isHapticEnabled = value;
                    Preferences.Set("Haptic_Enabled", value);
                    OnPropertyChanged();
                }
            }
        }


        private void ApplyTheme(bool darkMode)
        {
            Application.Current.UserAppTheme = darkMode ? AppTheme.Dark : AppTheme.Light;
        }

        public SettingsViewModel()
        {
            IsTextToSpeechEnabled = Preferences.Get("TTS_Enabled", true);
            IsDarkModeEnabled = Preferences.Get("DarkMode_Enabled", false);
            IsHapticEnabled = Preferences.Get("Haptic_Enabled", true);
            ApplyTheme(IsDarkModeEnabled);

        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
    