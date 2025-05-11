using System.ComponentModel.DataAnnotations;

namespace WebApplication8.ViewModels.Account
{
    public class RegisterVM
    {
        [Required, MaxLength(15), MinLength(5)]
        public string Name { get; set; }
        [MaxLength(10), MinLength(6)]
        public string UserName { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
