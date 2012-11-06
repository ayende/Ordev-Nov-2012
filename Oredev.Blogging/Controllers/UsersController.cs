using System;
using System.Linq;
using System.Web.Mvc;
using Oredev.Blogging.Models;

namespace Oredev.Blogging.Controllers
{
	public class UsersController : RavenController
	{
		private readonly string[] _firstName = new[]
		                                  	{
		                                  		"John", 
												"Lisa", 
												"Erica", 
												"Steven",
												"Alan"
		                                  	};

		private readonly string[] _lastName = new[]
		                                  	{
		                                  		"Doe", 
												"McFlurry", 
												"Watson", 
												"Turing"
		                                  	};

		readonly Random _rand = new Random();

		private object New()
		{
			var firstName = _firstName[_rand.Next() % _firstName.Length];
			var lastName = _lastName[_rand.Next() % _lastName.Length];

			var user = new User
						{
							Id = "users/",
							FullName = firstName + " " + lastName
						};

			Session.Store(user);

			return user.Id;
		}

		public object RepeatNew(string title, int count)
		{
			for (var i = 0; i < count; i++)
			{
				New();
			}

			return "Created " + count + " users.";
		}

		public JsonResult WithName(string name)
		{
			var posts = Session
							.Query<User>()
							.Where(p => p.FullName.StartsWith(name))
							.OrderBy(u => u.Id)
							.ToList();

			return Json(posts);
		}
	}
}
