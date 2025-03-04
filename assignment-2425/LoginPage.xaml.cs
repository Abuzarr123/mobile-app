using Firebase.Auth;
using System;

namespace assignment_2425
{
    public partial class LoginPage : ContentPage
    {
        // Firebase Authentication
        private FirebaseAuthProvider authProvider;

        public LoginPage()
        {
            InitializeComponent();

            // Initialize FirebaseAuthProvider with Firebase API key
            authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyBVnVmjhhb3tdb8XaIT_31IsEwYjQjN980"));
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            // Navigate back to the MainPage
            await Navigation.PushAsync(new MainPage()); // Replace with your main app page
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Login Error", "Please enter both email and password.", "OK");
                return;
            }

            try
            {
                // Sign in with Firebase
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);

                // Navigate to the main app page after successful login
                await Navigation.PushAsync(new NutritionPage());
            }
            catch (FirebaseAuthException ex)
            {
                await DisplayAlert("Login Failed", "Error: " + ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Login Failed", "An unexpected error occurred: " + ex.Message, "OK");
            }
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Sign Up Error", "Please enter both email and password.", "OK");
                return;
            }

            if (password.Length < 6)
            {
                await DisplayAlert("Sign Up Error", "Password must be at least 6 characters long.", "OK");
                return;
            }

            try
            {
                // Create a new user with Firebase
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);

                // Navigate to the main app page after successful sign-up
                await Navigation.PushAsync(new NutritionPage());
            }
            catch (FirebaseAuthException ex)
            {
                await DisplayAlert("Sign Up Failed", "Error: " + ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Sign Up Failed", "An unexpected error occurred: " + ex.Message, "OK");
            }
        }
    }
}
