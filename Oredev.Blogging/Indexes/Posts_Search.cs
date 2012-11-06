using System.Linq;
using Oredev.Blogging.Models;
using Raven.Client.Indexes;

namespace Oredev.Blogging.Indexes
{
	public class Posts_Search : AbstractIndexCreationTask<Post>
	{
		public class Result
		{
			public string Title { get; set; }
			public string Created { get; set; }
			public string AuthorId { get; set; }
			public string AuthorFullName { get; set; }
		}

		public Posts_Search()
		{
			Map = posts => from post in posts
						   select new
									{
										post.Title,
										post.Created,
										AuthorId = post.Author,
										AuthorFullName = ""
									};

			TransformResults = (database, posts) => from post in posts
													let user = database.Load<User>(post.Author)
													select new
															{
																post.Title,
																post.Created,
																AuthorId = post.Author,
																AuthorFullName = user.FullName
															};
		}
	}
}
