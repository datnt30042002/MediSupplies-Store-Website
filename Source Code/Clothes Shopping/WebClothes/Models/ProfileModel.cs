using DTOLibrary.Models;
using System.ComponentModel.DataAnnotations;

namespace WebClothes.Models
{
    public class ProfileModel
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? newPassword { get; set; }
        public bool? Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Avatar { get; set; }
        public string oldPassword { get; set; }
        public string conFirmPassword { get; set; }
        public string? RoleName { get; set; }

        public List<User> Users { get; set; }
        public bool? Status { get; set; }
        public int TotalPages { get; set; } // Total number of pages for pagination
        public int CurrentPage { get; set; } // Current page number

        public int CompletionPercentage { get; set; }
    }
}