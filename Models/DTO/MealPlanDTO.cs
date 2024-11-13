namespace FoodFestAPI.Models.DTO
{
    public class MealPlanDTO
    {
        public string PlanName { get; set; }
        public string MealType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MealPlanDayId { get; set; }
        public int RecipeId { get; set; }
        public string UserID { get; set; }

        public IEnumerable<MealPlanDaysDTO> MealPlanDaysDTO { get; set; }
    }

    public class MealPlanDaysDTO
    {
        public int MealPlanId { get; set; }
        public DateTime Date { get; set; }
    }
}
