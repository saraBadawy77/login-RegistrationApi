using System.ComponentModel.DataAnnotations;

namespace loginApi.Models
{
    public class Login
    {

        [Required(ErrorMessage = "Please enter a username.")]
        [MinLength(3, ErrorMessage = " at least 3 characters long.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        public string Password { get; set; }
       
    }
}
