using System.ComponentModel.DataAnnotations;


namespace WebClothes.Models
{
    public class LoginModel
    {
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

        public bool? Status { get; set; }
    }
}
