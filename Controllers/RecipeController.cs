using FoodFestAPI.Data;
using FoodFestAPI.Helpers;
using FoodFestAPI.Models;
using FoodFestAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FoodFestAPI.Controllers
{
    [Route("api/recipe")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly ApplicationDbContext _ctx;
        private ApiResponse _response;
        private readonly IImageService _imgService;
        private readonly ILogger<RecipeController> _log;

        public RecipeController(ApplicationDbContext ctx, IConfiguration config, IImageService imgService, ILogger<RecipeController> log)
        {
            _ctx = ctx;
            _response = new ApiResponse();
            _imgService = imgService;
            _log = log;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRecipe()
        {
            try
            {
                var result = await _ctx.Recipes
                    .Include(i => i.Ingredients)
                    .Include(i => i.Instructions)
                    .ToListAsync();

                List<Recipe> lrecipe = new List<Recipe>();
                List<Ingredient> lingredient = new List<Ingredient>();
                List<Instructions> linstructions = new List<Instructions>();

                foreach (var item in result)
                {

                    if (item.Ingredients.Count > 0)
                    {
                        lingredient.Add(new Ingredient
                        {
                            Name = item.Ingredients.FirstOrDefault(i => i.RecipeId == item.Id).Name,
                            Description = item.Ingredients.FirstOrDefault(i => i.RecipeId == item.Id).Description,
                            RecipeId = item.Id
                        });
                    }

                    if (item.Instructions.Count > 0)
                    {
                        linstructions.Add(new Instructions
                        {
                            StepNumber = item.Instructions.FirstOrDefault(i => i.RecipeId == item.Id).StepNumber,
                            Description = item.Instructions.FirstOrDefault(i => i.RecipeId == item.Id).Description,
                            RecipeId = item.Id
                        });
                    }

                    lrecipe.Add(new Recipe
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        CookingTime = item.CookingTime,
                        ServiceSize = item.ServiceSize,
                        ImageUrl = item.ImageUrl,
                        VideoUrl = item.VideoUrl,
                        UserId = item.UserId,
                        CategoriesId = item.CategoriesId,
                        Ingredients = lingredient,
                        Instructions = linstructions,
                    });
                };

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = result;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("{id:int}", Name = "GetRecipesByID")]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            var recipeById = await _ctx.Recipes.Include(i => i.Ingredients).Include(i => i.Instructions).FirstOrDefaultAsync(r => r.Id == id);

            if (recipeById == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound();
            }

            _response.Result = recipeById;
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateReceipe([FromBody] RecipeCreate request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (request.ImageUrl == null)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                    }

                    byte[] byteImg = Convert.FromBase64String(request.ImageUrl);
                    var stream = new MemoryStream(byteImg);
                    IFormFile fileResult = new FormFile(stream, 0, stream.Length, "name", "fileName");
                    var imgResult = await _imgService.AddImageAsync(fileResult);

                    Recipe recipe = new()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        CookingTime = request.CookingTime,
                        ServiceSize = request.ServiceSize,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        UserId = request.UserId,
                        ImageUrl = imgResult.Url.ToString(),
                        VideoUrl = request.VideoUrl,
                        //CategoriesId = request.CategoriesId
                    };

                    _ctx.Recipes.Add(recipe);

                    foreach (var reqIngredient in request.Ingredient)
                    {
                        Ingredient ingredientData = new()
                        {
                            Name = reqIngredient.Name,
                            Unit = reqIngredient.Unit,
                            Description = reqIngredient.Description,
                            UpdatedAt = DateTime.UtcNow,
                            CreatedAt = DateTime.UtcNow,
                            Recipe = recipe,
                        };
                        _ctx.Ingredients.Add(ingredientData);
                    }

                    foreach (var insItem in request.Instructions)
                    {
                        Instructions instData = new Instructions()
                        {
                            StepNumber = insItem.StepNumber,
                            Description = insItem.Description,
                            Recipe = recipe,
                        };
                        _ctx.Instructions.Add(instData);
                    }

                    await _ctx.SaveChangesAsync();

                    _response.Result = recipe;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateRecipe([FromBody] RecipeUpdateDTO request, int id)
        {
            try
            {
                var recipe = _ctx.Recipes
                    .Include(i => i.Ingredients)
                    .Include(i => i.Instructions)
                    .FirstOrDefault(r => r.Id == id);

                if (recipe != null)
                {
                    byte[] byteImg = Convert.FromBase64String(request.ImageUrl);
                    var stream = new MemoryStream(byteImg);
                    IFormFile fileResult = new FormFile(stream, 0, stream.Length, "name", "fileName");

                    if (recipe.ImageUrl != null)
                    {
                        var imgDelete = await _imgService.DeleteImageAsync(fileResult.ToString());
                        var imgResult = await _imgService.AddImageAsync(fileResult);
                        recipe.ImageUrl = imgResult.Url.ToString();
                    }

                    recipe.Name = request.Name;
                    recipe.Description = request.Description;
                    recipe.CookingTime = request.CookingTime;
                    recipe.ServiceSize = request.ServiceSize;
                    recipe.CreatedAt = DateTime.UtcNow;
                    recipe.UpdatedAt = DateTime.UtcNow;
                    recipe.UserId = request.UserId;
                    recipe.VideoUrl = request.VideoUrl;
                    _ctx.SaveChanges();

                    var ingredients = recipe.Ingredients.Where(i => i.RecipeId == recipe.Id).ToList();
                    if (ingredients == null)
                    {
                        foreach (var reqIngredient in request.Ingredient)
                        {
                            Ingredient ingredientData = new()
                            {
                                Name = reqIngredient.Name,
                                Unit = reqIngredient.Unit,
                                Description = reqIngredient.Description,
                                UpdatedAt = DateTime.UtcNow,
                                CreatedAt = DateTime.UtcNow,
                                Recipe = recipe,
                            };
                            _ctx.Ingredients.Add(ingredientData);
                        }
                    }
                    else
                    {
                        foreach (var reqIngredient in request.Ingredient)
                        {
                            var getIngredient = ingredients.FirstOrDefault(x => x.Id == reqIngredient.Id);
                            if (getIngredient != null)
                            {
                                getIngredient.Name = reqIngredient.Name;
                                getIngredient.Unit = reqIngredient.Unit;
                                getIngredient.Description = reqIngredient.Description;
                                getIngredient.UpdatedAt = DateTime.UtcNow;
                                _ctx.Ingredients.Update(getIngredient);
                            }
                            else
                            {
                                var ingredientEntity = new Ingredient()
                                {
                                    Name = reqIngredient.Name,
                                    Description = reqIngredient.Description,
                                    Unit = reqIngredient.Unit,
                                    UpdatedAt = DateTime.Now,
                                    Recipe = recipe
                                };

                                _ctx.Ingredients.Add(ingredientEntity);
                            }
                        }
                        var removeIngredients = recipe.Ingredients.Where(i => !request.Ingredient.Any(ui => ui.Id == i.Id)).ToList();
                        _ctx.Ingredients.RemoveRange(removeIngredients);
                        _ctx.SaveChanges();
                    }

                    var instructions = recipe.Instructions.Where(i => i.RecipeId == recipe.Id).ToList();
                    if (instructions == null)
                    {
                        foreach (var insItem in request.Instructions)
                        {
                            Instructions instData = new Instructions()
                            {
                                StepNumber = insItem.StepNumber,
                                Description = insItem.Description,
                                Recipe = recipe
                            };
                            _ctx.Instructions.Add(instData);
                        }
                    }
                    else
                    {
                        foreach (var reqInstruction in request.Instructions)
                        {
                            var getInstruction = instructions.FirstOrDefault(i => i.Id == reqInstruction.Id);
                            if (getInstruction != null)
                            {
                                getInstruction.StepNumber = reqInstruction.StepNumber;
                                getInstruction.Description = reqInstruction.Description;
                                _ctx.Instructions.Update(getInstruction);
                            }
                            else
                            {
                                Instructions insModel = new Instructions()
                                {
                                    StepNumber = reqInstruction.StepNumber,
                                    Description = reqInstruction.Description,
                                    Recipe = recipe
                                };
                                _ctx.Instructions.Add(insModel);
                            }
                        }

                        var removeInstruction = recipe.Instructions.Where(i => !request.Instructions.Any(ui => ui.Id == i.Id)).ToList();
                        _ctx.Instructions.RemoveRange(removeInstruction);
                        _ctx.SaveChanges();
                    }
                }
                else
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { };
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteRecipe(int recipeId)
        {
            var execStrategy = _ctx.Database.CreateExecutionStrategy();
            execStrategy.Execute(() =>
            {
                using var transaction = _ctx.Database.BeginTransaction();
                try
                {
                    var recipe = _ctx.Recipes
                        .Include(i => i.Ingredients)
                        .Include(i => i.Instructions)
                        .FirstOrDefault(r => r.Id == recipeId);

                    if (recipe != null)
                    {
                        _ctx.Ingredients.RemoveRange(recipe.Ingredients);
                        _ctx.Instructions.RemoveRange(recipe.Instructions);
                        _ctx.Recipes.Remove(recipe);

                        _ctx.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        _response.StatusCode = HttpStatusCode.NotFound;
                        _response.IsSuccess = false;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { ex.ToString() };
                }
            });

            return _response;
        }       
    }
}
