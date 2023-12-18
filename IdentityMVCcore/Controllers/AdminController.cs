using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMVCcore.Controllers
{
	[Authorize(Roles ="admin")]
	public class AdminController : Controller
	{
		public IActionResult Display()
		{
			return View();
		}
	}
}
