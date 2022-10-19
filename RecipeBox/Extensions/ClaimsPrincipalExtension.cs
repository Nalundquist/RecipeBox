using System.Linq;
using System.Security.Claims;

namespace RecipeBox.Extensions
{
	public static class ClaimsPrincipalExtension
	{
		public static string GetFullName(this ClaimsPrincipal principal)
		{
			var fullName = principal.Claims.FirstOrDefault(c => c.Type == "FullName");
			return fullName?.Value;
		}
		public static string GetDOB(this ClaimsPrincipal principal)
		{
			var birthDate = principal.Claims.FirstOrDefault(c => c.Type == "DOB");
			return birthDate?.Value;
		}
	}
}