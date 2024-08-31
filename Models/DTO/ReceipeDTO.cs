using System.ComponentModel.DataAnnotations.Schema;

namespace FoodFestAPI.Models.DTO
{
    public class RecipeCreate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CookingTime { get; set; }
        public string ServiceSize { get; set; }
        public string UserId { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        //public int CategoriesId { get; set; }
        
        public IEnumerable<IngredientCreate> Ingredient { get; set; }
        public IEnumerable<InstructionCreate> Instructions { get; set; }
    }

    public class RecipeUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CookingTime { get; set; }
        public string ServiceSize { get; set; }
        public string UserId { get; set; }
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        //public int CategoriesId { get; set; }

        public IEnumerable<IngredientUpdate> Ingredient { get; set; }
        public IEnumerable<InstructionUpdate> Instructions { get; set; }
    }

    public class IngredientCreate
    {
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        //public int RecipeId { get; set; }
    }

    public class InstructionCreate
    {
        public int StepNumber { get; set; }
        public string Description { get; set; }
        //public int RecipeId { get; set; }
    }

    public class IngredientUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public int RecipeId { get; set; }
        public DateTime UpdatedAt {  get; set; }
    }

    public class InstructionUpdate
    {
        public int Id { get; set; }
        public int StepNumber { get; set; }
        public string Description { get; set; }
        public int RecipeId { get; set; }
    }

    public class InstructionIngredientDelete
    {
        public int IngredientId { get; set; }
        public int InstructionId { get; set; }
    }
}
