using System;
using System.Collections.Generic;

namespace Oredev.Blogging.Models
{
	public class Order
	{
		public string Id { get; set; }
		public string RecipientId { get; set; }
		public DateTime Created { get; set; }
		private IList<OrderItem> _items;
		public IList<OrderItem> Items
		{
			get { return _items ?? (_items = new List<OrderItem>()); }
			set { _items = value; }
		}

		public bool IsComplete { get; set; }
	}

	public class OrderItem
	{
		public string ItemId { get; set; }
		public int Quantity { get; set; }
		public int Cost { get; set; }
	}

	public class Item
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public int Price { get; set; }
		public string[] Tags { get; set; }
	}
}
