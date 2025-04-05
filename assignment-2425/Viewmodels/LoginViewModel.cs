using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
using System;
using System.Threading.Tasks;

namespace assignment_2425.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly FirebaseAuthProvider _authProvider; //firebase authentication

        [ObservableProperty] // property in place of 
        private string email;

        [ObservableProperty]
        private string password;

        public LoginViewModel()
        {
            _authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyBVnVmjhhb3tdb8XaIT_31IsEwYjQjN980")); //initialising the authprovider with the firebase key
        }

        [RelayCommand]
        private async Task Login()
        {
            HapticFeedback.Perform(HapticFeedbackType.Click);

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await ShowAlert("Login Error", "Please enter both email and password.");
                return;
            }

            try
            {
                var auth = await _authProvider.SignInWithEmailAndPasswordAsync(Email, Password); //uses email and password to sign in
                await SecureStorage.SetAsync("firebase_uid", auth.User.LocalId);

                await ShowAlert("Success", "You are now logged in!");
                await Shell.Current.GoToAsync("//NutritionPage");
            }
            catch (FirebaseAuthException ex)
            {
                string message = ex.Reason switch // string of error messages that are built in firebase auth
                {
                    AuthErrorReason.WrongPassword => "Incorrect password. Please try again.",
                    AuthErrorReason.UnknownEmailAddress => "Email address not found. Please check or sign up.",
                    AuthErrorReason.InvalidEmailAddress => "That doesn't look like a valid email address.",
                    AuthErrorReason.MissingEmail => "Account does not exist. Please sign up first.",
                    _ => "Login failed. Please check your details and try again."
                };

                await ShowAlert("Login Failed", message);
            }
            catch
            {
                await ShowAlert("Error", "Something went wrong. Please try again.");
            }
        }

        [RelayCommand]
        private async Task SignUp()
        {
            HapticFeedback.Perform(HapticFeedbackType.Click); //hapric feedback for button click

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await TriggerWarningHaptic();
                await ShowAlert("Sign Up Error", "Please enter both email and password.");
                return;
            }

            if (Password.Length < 6)
            {
                await TriggerWarningHaptic();
                await ShowAlert("Sign Up Error", "Password must be at least 6 characters.");
                return;
            }

            try
            {
                await _authProvider.CreateUserWithEmailAndPasswordAsync(Email, Password);
                await TriggerSuccessHaptic();
                await ShowAlert("Success", "You have successfully signed up.");
            }
            catch (FirebaseAuthException ex)
            {
                await TriggerWarningHaptic();

                string message = ex.Reason switch
                {
                    AuthErrorReason.EmailExists => "That email is already in use. Try logging in instead.",
                    AuthErrorReason.InvalidEmailAddress => "Please enter a valid email address.",
                    _ => "Unable to sign up. Please check your info and try again."
                };

                await ShowAlert("Sign Up Failed", message);
            }
            catch
            {
                await TriggerWarningHaptic();
                await ShowAlert("Error", "Something went wrong. Please try again.");
            }
        }

        private async Task ShowAlert(string title, string message)
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
                Application.Current.MainPage.DisplayAlert(title, message, "OK"));
        }

        private async Task TriggerSuccessHaptic()
        {
            await RequestVibrationPermission();
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(100));
        }

        private async Task TriggerWarningHaptic()
        {
            await RequestVibrationPermission();
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(300));
        }

        private async Task RequestVibrationPermission()
        {
            var status = await Permissions.RequestAsync<Permissions.Vibrate>();
            if (status != PermissionStatus.Granted)
            {
                await ShowAlert("Permission Required", "Enable vibration for haptic feedback.");
            }
        }
    }
}
