using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserForRegisterDTO //DTOs are used to represent simplified version of our objects for the View
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Your password must be between 4 and 8 characters long.")]
        public string Password { get; set; }
    }
}