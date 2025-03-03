using Firebase.Auth;
using System.Diagnostics;

namespace assignment_2425
{
    public partial class LoginPage : ContentPage
    {
        // Firebase Authentication
        private FirebaseAuthProvider authProvider;

        public LoginPage()
        {
            InitializeComponent();

            // Initialize FirebaseAuthProvider with your Firebase API key
            authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyBVnVmjhhb3tdb8XaIT_31IsEwYjQjN980"));
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessageLabel.Text = "Please enter both email and password.";
                ErrorMessageLabel.IsVisible = true;
                return;
            }

            try
            {
                // Sign in with Firebase
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);

                // Navigate to the main app page after successful login
                await Navigation.PushAsync(new NutritionPage()); // Replace with your main app page
            }
            catch (FirebaseAuthException ex)
            {
                ErrorMessageLabel.Text = "Login failed: " + ex.Message;
                ErrorMessageLabel.IsVisible = true;
            }
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessageLabel.Text = "Please enter both email and password.";
                ErrorMessageLabel.IsVisible = true;
                return;
            }

            try
            {
                // Create a new user with Firebase
                var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);

                // Navigate to the main app page after successful sign-up
                await Navigation.PushAsync(new NutritionPage()); // Replace with your main app page
            }
            catch (FirebaseAuthException ex)
            {
                ErrorMessageLabel.Text = "Sign-up failed: " + ex.Message;
                ErrorMessageLabel.IsVisible = true;
            }
        }
    }
}