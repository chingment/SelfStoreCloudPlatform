using System.ComponentModel.DataAnnotations;

namespace WebAgent.Models.Home
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "Account")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "RememberMe?")]
        public bool IsRememberMe { get; set; }
    }
}
