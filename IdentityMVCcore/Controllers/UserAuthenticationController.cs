using IdentityMVCcore.Models.DTO;
using IdentityMVCcore.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMVCcore.Controllers
{
	public class UserAuthenticationController : Controller
	{
		private readonly IUserAuthenticationService service;

		public UserAuthenticationController(IUserAuthenticationService service)
        {
			this.service = service;
		}

		public IActionResult Registration()
		{
			return View();
		}
		[HttpPost]
		public async Task <IActionResult> Registration(RegistrationModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			model.Role = "user";
			var result = await service.RegisterAsync(model);
			TempData["msg"] = result.Message;
			return RedirectToAction(nameof(Login));
		}

		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login (LoginModel model)
		{
			if(!ModelState.IsValid)
			{
				return View(model);
			}
			var result = await service.LoginAsync(model);
			if (result.StatusCode == 1)
			{
				return RedirectToAction("Display", "Dashboard");
			}
			else
			{
				TempData["msg"] = result.Message;
				return View(nameof(Login));
			}
		}
		//logout
		[Authorize]
		public async Task <IActionResult> Logout()
		{
			await service.LogoutAsync();
			return RedirectToAction(nameof(Login));
		}
        //register Admin 
        //public async Task<IActionResult> Reg()
        //{
        //	var model = new RegistrationModel
        //	{
        //		UserName="admin",
        //		Name = "Muhammed Ahmed",
        //		Email="Moh@gmail.com",
        //		Password="Admin@12345#",
        //	};
        //	model.Role = "admin";
        //	var result = await service.RegisterAsync(model);
        //	return Ok(result);
        //}

        //change password
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await service.ChangePasswordAsync(model, User?.Identity?.Name!);
            TempData["msg"] = result.Message;
            return RedirectToAction("Display","Dashboard");
        }
    }
}
