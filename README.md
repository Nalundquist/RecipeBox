# Recipe Box

### Created by Shaniza Lingle, Harold Mesa, and Noah Lundquist in October of 2022

## Links

* [Repository](https://github.com/nalundquist/RecipeBox)

## Description

An interface for storing recipes.  Has login functionality with unique Categories/Recipes for individual users, ability to search by listed ingredient, ability to rate and sort by rating, and other lil nice bits.  CSS styling is nil at the time of this writing but that may change.

## Features

* Stores Categories and Recipes in tables
* Allows for creation of user accounts
* Has search, sort, and other functionalities
* full CRUD in recipes


## Technologies Used

* Built in VS Code (v.1.70.1) using the following languages:
	* C#
	* HTML
	* CSS

* ASP.Net Core
* MySQL Database
* MySQL Workbench
* Entity Framework

## Installation

* Download [Git Bash](https://git-scm.com/downloads)
* Input the following into Git Bash to clone this repository onto your computer:

		>git clone https://github.com/nalundquist/RecipeBox


* Enter the cloned project folder "/RecipeBox/RecipeBox" and type:

		>dotnet restore

followed by

		>dotnet build

*After this, create an appsettings.json file in the root /RecipeBox folder (sub in your own set username and password for the bracketed bits)

		{
  		"ConnectionStrings": {
      	"DefaultConnection": "Server=localhost;Port=3306;database=recipebox;uid=[USERNAME];pwd=[PASSWORD];"
  		}
		}

* next, type into console in the root directory:

		>dotnet ef database update

* Finally, navigate to the /Factory directory in git bash (if you navigated away) and type  

		>dotnet run

Follwing this you can navigate to http://localhost:5000 in the browser of your choice to navigate around the interface.  

## Known Bugs

For some reason User.Identity._____ does not point to what I generally understand to be the default table for such (RecipeBox.Models.ApplicationUser in this case) and as such we were unable to call any more than a Name field for the user on the account index page.

//FIXED// 

Added a claim extension to grab other fields from the signed in user.

## License

Licensed under [GNU GPL 3.0](https://www.gnu.org/licenses/gpl-3.0.en.html)
