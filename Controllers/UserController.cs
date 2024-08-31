using Azure;
using FoodFestAPI.Data;
using FoodFestAPI.Helpers;
using FoodFestAPI.Models;
using FoodFestAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FoodFestAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _ctx;
        private ApiResponse _response;
        private readonly IImageService _imgService;
        public UserController(ApplicationDbContext ctx, IConfiguration config, IImageService imgService) 
        {
            _ctx = ctx;
            _response = new ApiResponse();
            _imgService = imgService;
        }

        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var userId = await _ctx.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (userId == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _response.Result = userId;
            _response.IsSuccess = true;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateUser([FromBody] UserUpdateDTO request, string id)
        {
            try
            {
                var user = _ctx.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (user.Result != null)
                {
                    if (ModelState.IsValid)
                    {
                        byte[] byteImg = Convert.FromBase64String(request.ImageUrl);
                        var stream = new MemoryStream(byteImg);
                        IFormFile fileResult = new FormFile(stream, 0, stream.Length, "name", "fileName");

                        if (user.Result.ImageUrl != null)
                        {
                            var imgDelete = await _imgService.DeleteImageAsync(fileResult.ToString());
                            var imgResult = await _imgService.AddImageAsync(fileResult);
                            user.Result.ImageUrl = imgResult.Url.ToString();
                        }
                        else
                        {
                            var imgResult = await _imgService.AddImageAsync(fileResult);
                            user.Result.ImageUrl = imgResult.Url.ToString();
                        }

                        user.Result.Name        = request.Name;
                        user.Result.Email       = request.Email;
                        user.Result.Country     = request.Country;
                        user.Result.City        = request.City;
                        user.Result.PhoneNumber = request.PhoneNumber;
                        user.Result.SocialMedia = request.SocialMedia;
                        user.Result.Gender      = request.Gender;
                        _ctx.SaveChanges();

                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        _response.Result = request;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.IsSuccess = false;
                    }
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.InternalServerError;
                }
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode= HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }
    }
}
