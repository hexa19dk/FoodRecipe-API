using FoodFestAPI.Data;
using FoodFestAPI.Models;
using FoodFestAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace FoodFestAPI.Controllers
{
    [Route("api/mealPlan")]
    [ApiController]
    public class MealPlanController : ControllerBase
    {
        private readonly ApplicationDbContext _ctx;
        private ApiResponse _response;
        private readonly ILogger<MealPlanController> _log;
        public MealPlanController(ApplicationDbContext ctx, IConfiguration config, ILogger<MealPlanController> log)
        {
            _ctx = ctx;
            _response = new ApiResponse();
            _log = log;
        }

        [HttpGet]
        public async Task<IActionResult> GetMealPlans(string userId)
        {
            var getMealDay = await _ctx.MealPlans.Include(md => md.MealPlanDays).Where(ui => ui.UserID == userId).ToListAsync();

            if (getMealDay == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _response.Result = getMealDay;
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("/getByDay")]
        public async Task<ActionResult<ApiResponse>> GetMealplanByDay(string userId, string mealDate)
        {
            var result = _ctx.MealPlanDays
                .Where(md => md.Date == Convert.ToDateTime(mealDate) && md.MealPlans.UserID == userId) // Filter by date and user ID
                .Select(md => new
                {
                    UserId      = md.MealPlans.UserID,
                    RecipeName  = md.MealPlans.Recipe.Name,
                    CookingTime = md.MealPlans.Recipe.CookingTime,
                    ServiceSize = md.MealPlans.Recipe.ServiceSize,
                    MealType    = md.MealPlans.MealType,
                    Date        = md.Date
                }).ToList();

            _response.Result = result;
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateMealPlan([FromBody] MealPlanDTO request)
        {
            try
            {
                MealPlans mealPlan = new()
                {
                    PlanName    = request.PlanName,
                    MealType    = request.MealType,
                    StartDate   = request.StartDate,
                    EndDate     = request.EndDate,
                    RecipeId    = request.RecipeId,
                    UserID      = request.UserID,
                };

                _ctx.MealPlans.Add(mealPlan);

                foreach(var mealPDays in request.MealPlanDaysDTO)
                {
                    MealPlanDays mealPlanDays = new()
                    {
                        MealPlans = mealPlan,
                        Date = mealPDays.Date
                    };
                    _ctx.MealPlanDays.Add(mealPlanDays);
                }

                await _ctx.SaveChangesAsync();

                _response.Result = mealPlan;
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
            }
            catch(Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }
        
        [HttpDelete]
        public async Task<ActionResult<ApiResponse>> RemoveMealPlanByRecipeId(int recipeId, string userId)
        {
            var rmMealPlan = await _ctx.MealPlans.Include(md => md.MealPlanDays).FirstOrDefaultAsync(ui => ui.UserID == userId &&  ui.RecipeId == recipeId);

            if (rmMealPlan == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
    }
}
