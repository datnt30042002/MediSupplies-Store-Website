using System.ComponentModel.DataAnnotations;

namespace WebClothes.Models
{
    public class RegisterModel
    {
        [Required]
        public string FullName { get; set; }   

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        
    }
}
