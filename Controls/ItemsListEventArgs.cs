using System;

namespace smartRestaurant.Controls
{
	public class ItemsListEventArgs : EventArgs
	{
		private DataItem item;

		public DataItem Item
		{
			get
			{
				return this.item;
			}
		}

		public ItemsListEventArgs(DataItem item)
		{
			this.item = item;
		}
	}
}