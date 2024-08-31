namespace FoodFestAPI.Models
{
    public class UserFavorite
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RecipeId { get; set; }
        public DateTime FavoriteOn { get; set; }

        public Recipe Recipe { get; set; }
    }
}
