using System.Linq;
using Oredev.Blogging.Models;
using Raven.Client.Indexes;

namespace Oredev.Blogging.Indexes
{
	public class Orders_SalesPerItems : AbstractMultiMapIndexCreationTask<Orders_SalesPerItems.Result>
	{
		public class Result
		{
			public string ItemId { get; set; }
			public int TotalCost { get; set; }
			public int Quantity { get; set; }
			public string[] Tags { get; set; }
		}

		public Orders_SalesPerItems()
		{
			AddMap<Order>(orders => from order in orders
			                        from item in order.Items
			                        select new
				                        {
					                        item.ItemId,
					                        item.Quantity,
					                        TotalCost = item.Cost,
					                        Tags = new string[0]
				                        });

			AddMap<Item>(items => from item in items
			                      select new
				                      {
										ItemId = item.Id,
										Quantity = 0,
										TotalCost = 0,
										item.Tags
				                      });

			Reduce = results => from result in results
			                    group result by result.ItemId into g
			                    select new
				                    {
					                    ItemId = g.Key,
										Quantity = g.Sum(i => i.Quantity),
					                    TotalCost = g.Sum(i => i.TotalCost),
										Tags = g.SelectMany(x=>x.Tags)
				                    };
		}
	}
}