using System.ComponentModel.DataAnnotations;

namespace IdentityMVCcore.Models.DTO
{
	public class RegistrationModel
	{
		[Required]
		public string Name { get; set; } = string.Empty;
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
		[Required]
		public string UserName { get; set; } = string.Empty;
		[Required]
		[RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Minimum length 6 and must contain  1 Uppercase,1 lowercase, 1 special character and 1 digit")]
		public string Password { get; set; } = string.Empty;
		[Required]
		[Compare("Password")]
		public string ConfirmPassword { get; set; } = string.Empty;
		public string? Role { get; set; } 
    }
}
