using System;

namespace Oredev.Blogging.Models
{
	public class Post
	{
		public string[] Tags { get; set; }
		public string Title { get; set; }
		public string Id { get; set; }
		public string Content { get; set; }
		public string Author { get; set; }
		public DateTime Created { get; set; }
	}
}
