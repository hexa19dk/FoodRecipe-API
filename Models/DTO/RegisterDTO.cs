using System.ComponentModel.DataAnnotations;

namespace FoodFestAPI.Models.DTO
{
    public class RegisterDTO
    {
    }

    public class RegisterRequestDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }

    public class RegisterResponseDTO
    {

    }
}
