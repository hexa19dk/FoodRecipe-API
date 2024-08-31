namespace FoodFestAPI.Models
{
    public class RecipeData
    {
        public Recipe Recipes { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Instructions> Instructions { get; set; }
    }    
}
