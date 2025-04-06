namespace assignment_2425.Models
{
    public class CalorieRecord
    {
        public string Date { get; set; }
        public string Calories { get; set; }
        public string FoodName { get; set; }

        public int protein { get; set; }
        public int carbohydrates { get; set; }
        public int fats { get; set; }

        public string Macronutrients => $"Protein: {protein}g | Carbs: {carbohydrates}g | Fats: {fats}g";
    }
}
