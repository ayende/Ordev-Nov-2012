using System.Linq;
using Oredev.Blogging.Models;
using Raven.Client.Indexes;

namespace Oredev.Blogging.Indexes
{
	public class Orders_ProfitPerUser : AbstractIndexCreationTask<Order, Orders_ProfitPerUser.Result>
	{
		public class Result
		{
			public string UserId { get; set; }
			public int TotalProfit { get; set; }
			public string FullName { get; set; }
		}

		public Orders_ProfitPerUser()
		{
			Map = orders => from order in orders
			                from item in order.Items
			                select new { UserId = order.RecipientId, TotalProfit = item.Cost };

			Reduce = results => from result in results
			                    group result by result.UserId into g
			                    select new { UserId = g.Key, TotalProfit = g.Sum(r => r.TotalProfit) };

			TransformResults = (database, results) =>
			                   from result in results
			                   let user = database.Load<User>(result.UserId)
			                   select new
				                   {
					                   result.UserId,
					                   user.FullName,
					                   result.TotalProfit
				                   };
		}
	}
}