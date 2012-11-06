using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Oredev.Blogging.Indexes;
using Oredev.Blogging.Models;
using Raven.Client.Linq;

namespace Oredev.Blogging.Controllers
{
	public class PostsController : RavenController
	{
		private readonly string[] _tags = new[]
		                                  	{
		                                  		"RavenDB", 
												".NET", 
												"NoSQL", 
												"Database", 
												"Oredev", 
												"OLAP", 
												"Design", 
												"Performance", 
												"OLTP"
		                                  	};

		readonly Random _rand = new Random();

		public string New(string title)
		{
			var post = new Post
						{
							Id = "posts/",
							Created = new DateTime(2012, _rand.Next(1, 12), _rand.Next(1, 28)),
							Title = title,
							Author = "users/" + _rand.Next(1, 100),
							Content = "Dummy",
							Tags = Enumerable
									.Range(0, _rand.Next(1, _tags.Length))
									.Select(x => _tags[_rand.Next() % _tags.Length])
									.Distinct()
									.ToArray()
						};

			Session.Store(post);

			return post.Id;
		}

		public object RepeatNew(string title, int count)
		{
			for (var i = 0; i < count; i++)
			{
				New(title);
			}

			return "Created " + count + " posts.";
		}

		public JsonResult WithTag(string tag)
		{
			var posts = Session
							.Query<Post>()
							.Where(p => p.Tags.Contains(tag))
							.ToList();

			return Json(posts);
		}

		public JsonResult ByUser(string id)
		{
			var userId = "users/" + id;
			var posts = Session
							.Query<Post>()
							.Where(p => p.Author == userId)
							.OrderByDescending(p => p.Created)
							.ToList();

			return Json(posts);
		}

		public JsonResult ByUserName(string name)
		{
			var userIds = Session
							.Query<User>()
							.Where(p => p.FullName.StartsWith(name))
							.Select(u => u.Id);

			var posts = Session
							.Query<Post>()
							.Where(p => p.Author.In(userIds))
							.OrderBy(p => p.Author)
							.ThenByDescending(p => p.Created)
							.ToList();

			return Json(posts);
		}

		public JsonResult WriteAndQuery(string title)
		{
			var id = New(title);
			Session.SaveChanges();

			var timer = new Stopwatch();
			timer.Start();
			Post post = null;
			while (post == null && timer.Elapsed < TimeSpan.FromSeconds(5))
			{
				// post = Session.Query<Post>().FirstOrDefault(p => p.Id == id);
				post = Session.Query<Post>().FirstOrDefault(p => p.Title == title);
			}
			timer.Stop();

			if (post != null)
				return Json("The new post was found in " + timer.ElapsedMilliseconds);

			return Json("The new post was not found within " + timer.ElapsedMilliseconds);
		}

		public JsonResult IncludeUserName()
		{
			var posts = Session
							.Query<Posts_Search.Result, Posts_Search>()
							.OrderByDescending(p => p.Created)
							.ToList();

			return Json(posts);
		}
	}
}
