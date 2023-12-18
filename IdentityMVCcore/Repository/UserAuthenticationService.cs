using IdentityMVCcore.Models.Domain;
using IdentityMVCcore.Models.DTO;
using IdentityMVCcore.Repository.Abstract;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityMVCcore.Repository
{
	public class UserAuthenticationService : IUserAuthenticationService
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly RoleManager<IdentityRole> roleManager;

		public UserAuthenticationService(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			RoleManager<IdentityRole> roleManager)
        {
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.roleManager = roleManager;
		}

        public async Task<Status> ChangePasswordAsync(ChangePasswordModel model, string username)
        {
            var status = new Status();

            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                status.Message = "User does not exist";
                status.StatusCode = 0;
                return status;
            }
            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword!, model.NewPassword!);
            if (result.Succeeded)
            {
                status.Message = "Password has updated successfully";
                status.StatusCode = 1;
            }
            else
            {
                status.Message = "Some error occcured";
                status.StatusCode = 0;
            }
            return status;
        }

        public async Task<Status> LoginAsync(LoginModel model)
		{
			var status = new Status();
			//1-check name 
			var user = await userManager.FindByNameAsync(model.UserName);
			if(user == null)
			{
				status.StatusCode = 0;
				status.Message = "Invalid username";
				return status;
			}
			//2-check password
			if(! await userManager.CheckPasswordAsync(user, model.Password))
			{
				status.StatusCode = 0;
				status.Message = "Invalid Password";
				return status;
			}
			//3- signin user
			var SignInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);
			//4-check if role exist or not 
			if (SignInResult.Succeeded)
			{
				var UserRoles = await userManager.GetRolesAsync(user);
				var AuthClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Name,user.UserName!)
				};
				foreach (var userrole in UserRoles)
				{
						AuthClaims.Add(new Claim(ClaimTypes.Name, userrole));
				}
				status.StatusCode = 1;
				status.Message = "Logged in successfully";
				return status;
			}
			else if (SignInResult.IsLockedOut) 
			{
				status.StatusCode = 0;
				status.Message = "User is locked out";
				return status;
			}
			else
			{
				status.StatusCode = 0;
				status.Message = "Error on logging in";
				return status;
			}
		}

		public async Task LogoutAsync()
		{
			await signInManager.SignOutAsync();	
		}

		public async Task<Status> RegisterAsync(RegistrationModel model)
		{
			var status = new Status();
			//1- check username is exist or not	and check
			var userExists = await userManager.FindByNameAsync(model.UserName);

			if(userExists != null)
			{
				status.StatusCode = 0;
				status.Message = "User is Already existed";
				return status;
			}

			//2- assign model to new application user
			var user = new ApplicationUser()
			{
				SecurityStamp =Guid.NewGuid().ToString(),
				Name=model.Name,
				UserName=model.UserName,
				Email=model.Email,
				EmailConfirmed=true,
			};
			//3- createAsync to user and check  
			var result = await userManager.CreateAsync(user,model.Password);
			if (!result.Succeeded)
			{
				status.StatusCode = 0;
				status.Message = "User creation failed";
				return status;
			}

			//4- check role 
			if(! await roleManager.RoleExistsAsync(model.Role!))
			{
				await roleManager.CreateAsync(new IdentityRole(model.Role!));
			}

			if(await roleManager.RoleExistsAsync(model.Role!))
			{
				await userManager.AddToRoleAsync(user, model.Role!);
			}
			status.StatusCode = 1;
			status.Message = "You have registered successfully";
			return status;
		}
	}
}
