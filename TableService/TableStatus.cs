using System;
using System.Xml.Serialization;

namespace smartRestaurant.TableService
{
	[XmlType(Namespace="http://ws.smartRestaurant.net")]
	public class TableStatus
	{
		public int TableID;

		public string TableName;

		public bool InUse;

		public bool IsPrintBill;

		public bool IsWaitingItem;

		public bool LockInUse;

		public TableStatus()
		{
		}
	}
}