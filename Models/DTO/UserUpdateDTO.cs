using System.ComponentModel.DataAnnotations;

namespace FoodFestAPI.Models.DTO
{
    public class UserUpdateDTO
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public string SocialMedia { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [MaxLength(12, ErrorMessage = "The number digit must 12 digit long")]
        public string PhoneNumber { get; set; }
    }
}
