using Firebase.Auth;
using Microsoft.Maui.Devices; // Required for Haptic Feedback & Vibration
using System;
using System.Threading.Tasks;

namespace assignment_2425
{
    public partial class LoginPage : ContentPage
    {
        // Firebase Authentication
        private FirebaseAuthProvider authProvider;

        public LoginPage()
        {
            InitializeComponent();


            // Initialize the Firebase API Key
            authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyBVnVmjhhb3tdb8XaIT_31IsEwYjQjN980"));

            Shell.SetNavBarIsVisible(this, true);
            NavigationPage.SetHasBackButton(this, true);

        }

       /* private async void OnBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage", true);
        }*/

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // Haptic Feedback for Button Click
            HapticFeedback.Perform(HapticFeedbackType.Click);

            string email = EmailEntry.Text?.Trim();
            string password = PasswordEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Login Error", "Please enter both email and password.", "OK");
                TriggerWarningHaptic(); // Warning feedback
                return;
            }

            try
            {
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);

                // Success Haptic Feedback
                TriggerSuccessHaptic();

                await Shell.Current.GoToAsync("//NutritionPage");

                
            }
            catch (FirebaseAuthException ex)
            {
                TriggerWarningHaptic(); // Warning feedback
                await DisplayAlert("Login Failed", "Error: " + ex.Message, "OK");
            }
            catch (Exception ex)
            {
                TriggerWarningHaptic(); // Warning feedback
                await DisplayAlert("Login Failed", "An unexpected error occurred: " + ex.Message, "OK");
            }
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            // Haptic Feedback for Button Click
            HapticFeedback.Perform(HapticFeedbackType.Click);

            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Sign Up Error", "Please enter both email and password.", "OK");
                TriggerWarningHaptic(); // Warning feedback
                return;
            }

            if (password.Length < 6)
            {
                await DisplayAlert("Sign Up Error", "Password must be at least 6 characters long.", "OK");
                TriggerWarningHaptic(); // Warning feedback
                return;
            }

            try
            {
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);

                // Success Haptic Feedback
                TriggerSuccessHaptic();

                await Navigation.PushAsync(new NutritionPage());
            }
            catch (FirebaseAuthException ex)
            {
                TriggerWarningHaptic(); // Warning feedback
                await DisplayAlert("Sign Up Failed", "Error: " + ex.Message, "OK");
            }
            catch (Exception ex)
            {
                TriggerWarningHaptic(); // Warning feedback
                await DisplayAlert("Sign Up Failed", "An unexpected error occurred: " + ex.Message, "OK");
            }
        }

        // Requesting permissions to use haptic feedback
        private async Task RequestVibrationPermission()
        {
            var status = await Permissions.RequestAsync<Permissions.Vibrate>();

            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Permission Required", "Vibration permission is needed for haptic feedback.", "OK");
            }
        }

        // Triggers hapticfeedback 
        private async void TriggerSuccessHaptic()
        {
            await RequestVibrationPermission();
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(100)); // Short vibration for success
        }

        // Gives warning for haptic feedback
        private async void TriggerWarningHaptic()
        {
            await RequestVibrationPermission();
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(300)); // Longer vibration for warning
        }
    }
}
