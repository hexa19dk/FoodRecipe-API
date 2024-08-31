using System.ComponentModel.DataAnnotations;

namespace FoodFestAPI.Models.DTO
{
    public class LoginDTO
    {
    }

    public class LoginRequestDTO 
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginResponseDTO 
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
