using System.ComponentModel.DataAnnotations;

namespace WebClothes.Models
{
    public class ChangePasswordModel
    {
        public string userID { get; set; }
 
        [Required]
        [DataType(DataType.Password)]
        public string oldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string conFirmPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }
    }
}
