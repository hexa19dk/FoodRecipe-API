using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace FoodFestAPI.Models
{
    public class MealPlans
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string MealType { get; set; }
        [Required]
        public string PlanName { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int RecipeId { get; set; }
        [Required]
        public string UserID { get; set; }

        public Recipe Recipe { get; set; }
        public ICollection<MealPlanDays> MealPlanDays { get; set; } 
        
    }
}
