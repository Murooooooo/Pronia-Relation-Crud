using System.ComponentModel.DataAnnotations;

namespace WebApplication8.Areas.ViewModels.Account
{
    public class LoginVM
    {
        [Required,StringLength(20)]
        public string UserNameOrEmail { get; set; }

        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
        
        public bool RememberMe {  get; set; }
    }
}
