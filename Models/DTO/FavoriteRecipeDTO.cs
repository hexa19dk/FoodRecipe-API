using System.ComponentModel.DataAnnotations;

namespace FoodFestAPI.Models.DTO
{
    public class FavoriteRecipeDTO
    {
        public int? FavoriteId { get; set; }
        public int RecipeId { get; set; }
        public string UserId { get; set; } = "";
        public string RecipeName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? FavoritedDate { get; set; }
    }

    public class ListFavorites
    {
        public List<FavoriteDTO> favoriteDTOs { get; set; }
    }

    public class FavoriteDTO
    {
        [Required]
        public int RecipeId { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime FavoriteOn { get; set; }
        public int IsFavorited { get; set; }
    }
}
