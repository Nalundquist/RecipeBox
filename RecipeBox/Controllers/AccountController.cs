using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using RecipeBox.Models;
using System.Threading.Tasks;
using RecipeBox.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System;

namespace RecipeBox.Controllers
{
	public class AccountController : Controller
	{
		private readonly RecipeBoxContext _db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RecipeBoxContext db)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_db = db;
		}

		public ActionResult Index()
		{
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Register (RegisterViewModel model)
		{
			var user = new ApplicationUser { UserName = model.UserName, FullName = model.FullName, DOB = model.DOB};
			IdentityResult result = await _userManager.CreateAsync(user, model.Password);
			if (result.Succeeded)
			{
				await _userManager.AddClaimAsync(user, new Claim("FullName", user.FullName));
				await _userManager.AddClaimAsync(user, new Claim("DOB", user.DOB.ToString("MM/dd/yyyy")));
				await _signInManager.SignInAsync(user, isPersistent: true);
				return RedirectToAction("Index");
			}
			else
			{
				return View();
			}
		}

		public ActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Login(LoginViewModel model)
		{
			Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: true, lockoutOnFailure: false);
			if (result.Succeeded)
			{
				return RedirectToAction("Index");
			}
			else
			{
				// if (model.AccessFailedCount > 3)
				// {
				// 	model.LockoutEnabled = true;
				// 	model.LockoutEnd = DateTimeOffset(DateTime.Now(), 0:00:03:00:00);
				// 	return RedirectToAction("Lockout");
				// }
				return View();
			}
		}

    [HttpPost]
    public async Task<ActionResult> LogOff()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index");
    }

		public ActionResult Lockout()
		{
			return View();
		}
	}
}