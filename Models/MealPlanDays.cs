using System.ComponentModel.DataAnnotations;

namespace FoodFestAPI.Models
{
    public class MealPlanDays
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int MealPlanId { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public MealPlans MealPlans { get; set; }
    }
}
