using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using RecipeBox.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RecipeBox.Controllers
{
  [Authorize]
  public class RecipesController : Controller
  {
    private readonly RecipeBoxContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public RecipesController(UserManager<ApplicationUser> userManager, RecipeBoxContext db)
    {
      _userManager = userManager;
      _db = db;
    }

		public async Task<ActionResult> Index(string sortOrder)
		{
			var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var currentUser = await _userManager.FindByIdAsync(userId);
			switch (sortOrder)
			{
				case "rating":
					var userRecipesRating = _db.Recipes.Where(entry => entry.User.Id == currentUser.Id).OrderByDescending(recipe => recipe.Rating).ToList();
					return View(userRecipesRating);
				default:
					var userRecipesName = _db.Recipes.Where(entry => entry.User.Id == currentUser.Id).OrderBy(recipe => recipe.Name).ToList();
					return View(userRecipesName);
			}
		}

		public async Task<ActionResult> Create()
		{
			var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var currentUser = await _userManager.FindByIdAsync(userId);
			List<Category> userCategories = _db.Categories.Where(entry => entry.User.Id == currentUser.Id).ToList();
			ViewBag.CategoryId = new SelectList(userCategories, "CategoryId", "Name");
			return View();
		}

		[HttpPost]
		public async Task<ActionResult> Create(Recipe recipe, int CategoryId)
		{
			var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var currentUser = await _userManager.FindByIdAsync(userId);
			recipe.User = currentUser;
			_db.Recipes.Add(recipe);
			_db.SaveChanges();
      
			if (CategoryId != 0)
			{
				_db.RecipeCategory.Add(new RecipeCategory() { CategoryId = CategoryId, RecipeId = recipe.RecipeId});
			}
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		public ActionResult Details(int id)
		{
			Recipe thisRecipe = _db.Recipes
				.Include(recipe => recipe.JoinEntities)
				.ThenInclude(join => join.Recipe)
				.FirstOrDefault(recipe => recipe.RecipeId == id);
			return View(thisRecipe);
		}

		public async Task<ActionResult> Edit(int id)
		{
			Recipe thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
			var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var currentUser = await _userManager.FindByIdAsync(userId);
			List<Category> userCategories = _db.Categories.Where(entry => entry.User.Id == currentUser.Id).ToList();
			ViewBag.CategoryId = new SelectList(userCategories, "CategoryId", "Name");
			return View(thisRecipe);
		}

		[HttpPost]
		public ActionResult Edit(Recipe recipe, int CategoryId)
		{
			_db.Entry(recipe).State = EntityState.Modified;
			_db.SaveChanges();

			foreach(RecipeCategory join in _db.RecipeCategory)
			{
				if(recipe.RecipeId == join.RecipeId && CategoryId == join.CategoryId)
				{
					return RedirectToAction("Details", new {id = recipe.RecipeId});
				}
			}
			if (CategoryId != 0)
			{
				_db.RecipeCategory.Add(new RecipeCategory() { CategoryId = CategoryId, RecipeId = recipe.RecipeId});
				_db.SaveChanges();
			}
			return RedirectToAction("Details", new {id = recipe.RecipeId});
		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			Recipe thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
			_db.Recipes.Remove(thisRecipe);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult DeleteCategory(int joinId)
		{
			RecipeCategory thisJoin = _db.RecipeCategory.FirstOrDefault(join => join.RecipeCategoryId == joinId);
			_db.RecipeCategory.Remove(thisJoin);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		public ActionResult Search(string query)
		{
			List<Recipe> thisSearch = _db.Recipes.Where(recipe => recipe.Ingredients.Contains(query)).ToList();
			ViewBag.SearchQuery = query;
			return View(thisSearch);
		}
  }
}


