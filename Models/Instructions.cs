using System.ComponentModel.DataAnnotations.Schema;

namespace FoodFestAPI.Models
{
    public class Instructions
    {
        public int Id { get; set; }
        public int StepNumber { get; set; }
        public string Description { get; set; }
        public int RecipeId { get; set; }
        [ForeignKey("RecipeId")]
        public Recipe Recipe { get; set; }
    }
}
