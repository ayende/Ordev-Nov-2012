using System;
using System.Web.Mvc;
using Oredev.Blogging.Indexes;
using Oredev.Blogging.Models;
using Raven.Client.Linq;
using System.Linq;
using Raven.Client;

namespace Oredev.Blogging.Controllers
{
	public class OrderController : RavenController
	{
		public object UserStats(string user)
		{
			var up = Session.Query<Orders_ProfitPerUser.Result, Orders_ProfitPerUser>()
				.Include(x => x.UserId)
				.FirstOrDefault(x => x.UserId == user);

			return Json(up);
		}

		public JsonResult CreateOrder(string recipientId)
		{
			var order = new Order { RecipientId = recipientId, Created = DateTime.UtcNow };
			Session.Store(order);

			return Json(order);
		}

		public JsonResult AddItem(int id, string itemId, int nrOfUnits, int cost)
		{
			var item = new OrderItem() { Quantity = nrOfUnits, Cost = cost, ItemId = itemId };

			var order = Session.Load<Order>(id);
			order.Items.Add(item);

			return Json(order);
		}

		public JsonResult MarkAsComplete(string orderId)
		{
			var order = Session.Load<Order>(orderId);
			order.IsComplete = true;

			return Json(order);
		}
	}
}
