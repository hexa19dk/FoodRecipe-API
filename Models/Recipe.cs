using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodFestAPI.Models
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CookingTime { get; set; }
        public string ServiceSize { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        [ForeignKey("CategoriesId")]
        public int? CategoriesId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Categories Categories { get; set; }
        public AppUser AppUser { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public ICollection<Instructions> Instructions { get; set; } = new List<Instructions>();
        public ICollection<UserFavorite> UserFavorites { get; set; } = new List<UserFavorite>();
        public ICollection<MealPlans> MealPlans { get; set; } = new List<MealPlans>();
    }
}
