﻿using FoodFestAPI.Data;
using FoodFestAPI.Helpers;
using FoodFestAPI.Models;
using FoodFestAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;

namespace FoodFestAPI.Controllers
{
    [Route("api/favorite")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly ApplicationDbContext _ctx;
        private ApiResponse _response;
        private readonly IImageService _imgService;
        private readonly ILogger<RecipeController> _log;

        public FavoriteController(ApplicationDbContext ctx, IConfiguration config, IImageService imgService, ILogger<RecipeController> log)
        {
            _ctx = ctx;
            _response = new ApiResponse();
            _imgService = imgService;
            _log = log;
        }

        [HttpGet("get-favoriteBy-userId/{userId?}")]
        public async Task<ActionResult<FavoriteRecipeDTO>> getFavoriteByUserId(string userId, int pageNumber, int pageSize)
        {
            try
            {
                if (userId != null)
                {
                    var query = from r in _ctx.Recipes
                                  join fr in _ctx.UserFavorites
                                  on new { RecipeId = r.Id, UserId = userId } equals new { RecipeId = fr.RecipeId, UserId = fr.UserId } into favorites
                                  from f in favorites.DefaultIfEmpty()
                                  select new
                                  {
                                      RecipeName = r.Name,
                                      FavoriteId = f != null ? f.Id : 0,
                                      RecipeId = r.Id,
                                      UserId = f != null ? f.UserId : null,
                                      Description = r.Description,
                                      ImageUrl = r.ImageUrl,
                                      CreatedAt = r.CreatedAt,
                                      IsFavorited = f != null ? 1 : 0
                                  };

                    int totalRecords = await query.CountAsync();

                    var favRecipePages = await query
                        .OrderBy(f => f.RecipeId)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

                    _response.StatusCode    = HttpStatusCode.OK;
                    _response.IsSuccess     = true;
                    _response.Result        = new
                    {
                        TotalRecords = totalRecords,
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                        FavoriteRecipes = favRecipePages
                    };
                }
                else
                {
                    var query = from r in _ctx.Recipes
                                join uf in _ctx.UserFavorites
                                on r.Id equals uf.RecipeId into userFavorites
                                from uf in userFavorites.DefaultIfEmpty()
                                select new
                                {
                                    RecipeName = r.Name,
                                    RecipeId = r.Id,
                                    FavoriteId = uf != null ? uf.Id : (int?)null,
                                    UserId = uf != null ? uf.UserId : (string)null,
                                    r.ImageUrl,
                                    r.Description,
                                    r.CreatedAt
                                };

                    int totalRecords = await query.CountAsync();

                    var favRecipePages = await query
                        .OrderBy(f => f.RecipeId)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

                    _response.StatusCode    = HttpStatusCode.OK;
                    _response.IsSuccess     = true;
                    _response.Result        = new
                    {
                        TotalRecords = totalRecords,
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                        FavoriteRecipes = favRecipePages
                    };
                }

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _log.LogInformation($"Internal server error, {ex.Message}");
                _response.IsSuccess     = false;
                _response.StatusCode    = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
                return StatusCode(500, _response);
            }
        }
        
        [HttpGet("get-by-recipe-id")]
        public async Task<ActionResult<UserFavorite>> getByRecipeId(int recipeId)
        {
            var checkFavRecipe = await _ctx.UserFavorites.FirstOrDefaultAsync(r => r.RecipeId == recipeId);

            if (checkFavRecipe == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound();
            }

            _response.Result = checkFavRecipe;
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpPost("add-remove")]
        public async Task<ActionResult<ApiResponse>> addRemove([FromBody] ListFavorites request)
        {
            try
            {
                foreach (var item in request.favoriteDTOs)
                {
                    if (item.RecipeId == 0 || item.UserId == null)
                    {
                        _response.StatusCode = HttpStatusCode.NotFound;
                        _response.IsSuccess = false;
                        _response.ErrorMessages ??= new List<string>();
                        _response.ErrorMessages.Add($"recipeId or userId not found {_response}");
                        continue;
                        //return BadRequest($"recipeId or userId not found {_response}");
                    }

                    var checkToFavorite = await _ctx.UserFavorites.FirstOrDefaultAsync(r => r.RecipeId == item.RecipeId && r.UserId == item.UserId);

                    if (checkToFavorite != null && item.IsFavorited == 1)
                    {
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                        _response.ErrorMessages ??= new List<string>();
                        _response.ErrorMessages.Add($"Bad request: data with userId: {item.UserId}, recipeId: {item.RecipeId}, and isFavorite: {item.IsFavorited} already exists");
                        continue;
                        //return BadRequest($"Bad request: data with userId: {item.UserId}, recipeId: {item.RecipeId}, and isFavorite: {item.IsFavorited}");                        
                    }

                    if (checkToFavorite != null)
                    {
                        await Remove(item);
                    }
                    else
                    {
                        await Add(item);
                    }
                }                

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _log.LogError($"Internal server error, {ex.Message}");
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPost("add")]
        public async Task<ActionResult<ApiResponse>> Add([FromBody] FavoriteDTO request)
        {
            try
            {
                UserFavorite fav = new UserFavorite()
                {
                    UserId = request.UserId,
                    RecipeId = request.RecipeId,
                    FavoriteOn = DateTime.Now
                };

                _ctx.UserFavorites.Add(fav);

                await _ctx.SaveChangesAsync();

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = fav;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPost("remove")]
        public async Task<ActionResult<ApiResponse>> Remove([FromBody] FavoriteDTO request)
        {
            try
            {
                var getFav = await _ctx.UserFavorites.FirstOrDefaultAsync(u => u.UserId == request.UserId && u.RecipeId == request.RecipeId);

                if (getFav == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                }

                _ctx.UserFavorites.Remove(getFav);
                await _ctx.SaveChangesAsync();

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }
    }
}
