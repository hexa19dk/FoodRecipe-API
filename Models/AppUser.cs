using Microsoft.AspNetCore.Identity;

namespace FoodFestAPI.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string SocialMedia { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
