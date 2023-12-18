using IdentityMVCcore.Models.DTO;

namespace IdentityMVCcore.Repository.Abstract
{
	public interface IUserAuthenticationService
	{
		Task<Status> LoginAsync(LoginModel model);
		Task<Status> RegisterAsync(RegistrationModel model);
		Task LogoutAsync();
        Task<Status> ChangePasswordAsync(ChangePasswordModel model, string username);
    }
}
