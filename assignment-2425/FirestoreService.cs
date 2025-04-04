using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace assignment_2425
{
    public class FirestoreService
    {
        private FirestoreDb db;

        public FirestoreService()
        {
            try
            {
                string credentialsPath = Path.Combine(FileSystem.AppDataDirectory, "firebase_credentials.json");

                if (!File.Exists(credentialsPath))
                    throw new FileNotFoundException("Firebase credentials file not found.");

                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);
                db = FirestoreDb.Create("nutritiontrackerapp-adabf");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Firestore Initialization Error: {ex.Message}");
            }
        }

        public async Task SaveCalorieData(string userId, int calories, string foodName)
        {
            try
            {
                DocumentReference docRef = db.Collection("users").Document(userId).Collection("calories").Document();
                Dictionary<string, object> calorieData = new Dictionary<string, object>
                {
                    { "date", DateTime.UtcNow.ToString("yyyy-MM-dd") },
                    { "calories", calories },
                    { "foodName", foodName }
                };
                await docRef.SetAsync(calorieData);
                Console.WriteLine("Calorie data saved successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }

        public async Task<List<CalorieRecord>> GetCalorieData(string userId)
        {
            List<CalorieRecord> calorieList = new List<CalorieRecord>();

            try
            {
                Query calorieQuery = db.Collection("users").Document(userId).Collection("calories");
                QuerySnapshot snapshot = await calorieQuery.GetSnapshotAsync();

                Console.WriteLine($"📄 Found {snapshot.Documents.Count} documents for {userId}");

                foreach (DocumentSnapshot doc in snapshot.Documents)
                {
                    Dictionary<string, object> data = doc.ToDictionary();

                    if (data.ContainsKey("date") && data.ContainsKey("calories"))
                    {
                        string date = data["date"].ToString();
                        int calories = Convert.ToInt32(data["calories"]);

                        string foodName = data.ContainsKey("foodName") ? data["foodName"].ToString() : "Unknown";

                        calorieList.Add(new CalorieRecord
                        {
                            Date = date,
                            Calories = $"{calories} kcal",
                            FoodName = foodName
                        });
                    }
                    else
                    {
                        Console.WriteLine("Skipped a document with missing fields");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving data: {ex.Message}");
            }

            return calorieList;
        }
    }
}
