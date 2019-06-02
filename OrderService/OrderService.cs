using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace smartRestaurant.OrderService
{
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[WebServiceBinding(Name="OrderServiceSoap", Namespace="http://ws.smartRestaurant.net")]
	public class OrderService : SoapHttpClientProtocol
	{
		public OrderService()
		{
			string item = ConfigurationSettings.AppSettings["smartRestaurant.OrderService.OrderService"];
			if (item == null)
			{
				base.Url = "http://localhost/smartService/OrderService.asmx";
				return;
			}
			base.Url = string.Concat(item, "");
		}

		public IAsyncResult BeginGetBillDetailWaitingList(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("GetBillDetailWaitingList", new object[0], callback, asyncState);
		}

		public IAsyncResult BeginGetCancelReason(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("GetCancelReason", new object[0], callback, asyncState);
		}

		public IAsyncResult BeginGetOrder(int tableID, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { tableID };
			return base.BeginInvoke("GetOrder", objArray, callback, asyncState);
		}

		public IAsyncResult BeginGetOrderByOrderID(int OrderID, AsyncCallback callback, object asyncState)
		{
			object[] orderID = new object[] { OrderID };
			return base.BeginInvoke("GetOrderByOrderID", orderID, callback, asyncState);
		}

		public IAsyncResult BeginGetTableReference(int orderID, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { orderID };
			return base.BeginInvoke("GetTableReference", objArray, callback, asyncState);
		}

		public IAsyncResult BeginGetTakeOutList(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("GetTakeOutList", new object[0], callback, asyncState);
		}

		public IAsyncResult BeginPrintBill(int OrderBillID, AsyncCallback callback, object asyncState)
		{
			object[] orderBillID = new object[] { OrderBillID };
			return base.BeginInvoke("PrintBill", orderBillID, callback, asyncState);
		}

		public IAsyncResult BeginPrintReceipt(int OrderBillID, AsyncCallback callback, object asyncState)
		{
			object[] orderBillID = new object[] { OrderBillID };
			return base.BeginInvoke("PrintReceipt", orderBillID, callback, asyncState);
		}

		public IAsyncResult BeginReprintBill(int OrderBillID, AsyncCallback callback, object asyncState)
		{
			object[] orderBillID = new object[] { OrderBillID };
			return base.BeginInvoke("ReprintBill", orderBillID, callback, asyncState);
		}

		public IAsyncResult BeginSendOrder(OrderInformation orderInfo, int CustID, string CustFullName, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { orderInfo, CustID, CustFullName };
			return base.BeginInvoke("SendOrder", objArray, callback, asyncState);
		}

		public IAsyncResult BeginSendOrderBill(OrderBill Bill, AsyncCallback callback, object asyncState)
		{
			object[] bill = new object[] { Bill };
			return base.BeginInvoke("SendOrderBill", bill, callback, asyncState);
		}

		public IAsyncResult BeginSendOrderPrint(OrderInformation orderInfo, int CustID, string CustFullName, bool print, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { orderInfo, CustID, CustFullName, print };
			return base.BeginInvoke("SendOrderPrint", objArray, callback, asyncState);
		}

		public IAsyncResult BeginServeWaitingOrder(int orderID, int billDetailID, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { orderID, billDetailID };
			return base.BeginInvoke("ServeWaitingOrder", objArray, callback, asyncState);
		}

		public IAsyncResult BeginSetTableReference(int orderID, int[] tableIDList, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { orderID, tableIDList };
			return base.BeginInvoke("SetTableReference", objArray, callback, asyncState);
		}

		public OrderWaiting[] EndGetBillDetailWaitingList(IAsyncResult asyncResult)
		{
			return (OrderWaiting[])base.EndInvoke(asyncResult)[0];
		}

		public CancelReason[] EndGetCancelReason(IAsyncResult asyncResult)
		{
			return (CancelReason[])base.EndInvoke(asyncResult)[0];
		}

		public OrderInformation EndGetOrder(IAsyncResult asyncResult)
		{
			return (OrderInformation)base.EndInvoke(asyncResult)[0];
		}

		public OrderInformation EndGetOrderByOrderID(IAsyncResult asyncResult)
		{
			return (OrderInformation)base.EndInvoke(asyncResult)[0];
		}

		public int[] EndGetTableReference(IAsyncResult asyncResult)
		{
			return (int[])base.EndInvoke(asyncResult)[0];
		}

		public TakeOutInformation[] EndGetTakeOutList(IAsyncResult asyncResult)
		{
			return (TakeOutInformation[])base.EndInvoke(asyncResult)[0];
		}

		public int EndPrintBill(IAsyncResult asyncResult)
		{
			return (int)base.EndInvoke(asyncResult)[0];
		}

		public int EndPrintReceipt(IAsyncResult asyncResult)
		{
			return (int)base.EndInvoke(asyncResult)[0];
		}

		public int EndReprintBill(IAsyncResult asyncResult)
		{
			return (int)base.EndInvoke(asyncResult)[0];
		}

		public string EndSendOrder(IAsyncResult asyncResult)
		{
			return (string)base.EndInvoke(asyncResult)[0];
		}

		public string EndSendOrderBill(IAsyncResult asyncResult)
		{
			return (string)base.EndInvoke(asyncResult)[0];
		}

		public string EndSendOrderPrint(IAsyncResult asyncResult)
		{
			return (string)base.EndInvoke(asyncResult)[0];
		}

		public OrderWaiting[] EndServeWaitingOrder(IAsyncResult asyncResult)
		{
			return (OrderWaiting[])base.EndInvoke(asyncResult)[0];
		}

		public int EndSetTableReference(IAsyncResult asyncResult)
		{
			return (int)base.EndInvoke(asyncResult)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetBillDetailWaitingList", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public OrderWaiting[] GetBillDetailWaitingList()
		{
			object[] objArray = base.Invoke("GetBillDetailWaitingList", new object[0]);
			return (OrderWaiting[])objArray[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetCancelReason", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public CancelReason[] GetCancelReason()
		{
			object[] objArray = base.Invoke("GetCancelReason", new object[0]);
			return (CancelReason[])objArray[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetOrder", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public OrderInformation GetOrder(int tableID)
		{
			object[] objArray = new object[] { tableID };
			return (OrderInformation)base.Invoke("GetOrder", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetOrderByOrderID", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public OrderInformation GetOrderByOrderID(int OrderID)
		{
			object[] orderID = new object[] { OrderID };
			return (OrderInformation)base.Invoke("GetOrderByOrderID", orderID)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetTableReference", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public int[] GetTableReference(int orderID)
		{
			object[] objArray = new object[] { orderID };
			return (int[])base.Invoke("GetTableReference", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetTakeOutList", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public TakeOutInformation[] GetTakeOutList()
		{
			object[] objArray = base.Invoke("GetTakeOutList", new object[0]);
			return (TakeOutInformation[])objArray[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/PrintBill", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public int PrintBill(int OrderBillID)
		{
			object[] orderBillID = new object[] { OrderBillID };
			return (int)base.Invoke("PrintBill", orderBillID)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/PrintReceipt", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public int PrintReceipt(int OrderBillID)
		{
			object[] orderBillID = new object[] { OrderBillID };
			return (int)base.Invoke("PrintReceipt", orderBillID)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/ReprintBill", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public int ReprintBill(int OrderBillID)
		{
			object[] orderBillID = new object[] { OrderBillID };
			return (int)base.Invoke("ReprintBill", orderBillID)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/SendOrder", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string SendOrder(OrderInformation orderInfo, int CustID, string CustFullName)
		{
			object[] objArray = new object[] { orderInfo, CustID, CustFullName };
			return (string)base.Invoke("SendOrder", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/SendOrderBill", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string SendOrderBill(OrderBill Bill)
		{
			object[] bill = new object[] { Bill };
			return (string)base.Invoke("SendOrderBill", bill)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/SendOrderPrint", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string SendOrderPrint(OrderInformation orderInfo, int CustID, string CustFullName, bool print)
		{
			object[] objArray = new object[] { orderInfo, CustID, CustFullName, print };
			return (string)base.Invoke("SendOrderPrint", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/ServeWaitingOrder", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public OrderWaiting[] ServeWaitingOrder(int orderID, int billDetailID)
		{
			object[] objArray = new object[] { orderID, billDetailID };
			return (OrderWaiting[])base.Invoke("ServeWaitingOrder", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/SetTableReference", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public int SetTableReference(int orderID, int[] tableIDList)
		{
			object[] objArray = new object[] { orderID, tableIDList };
			return (int)base.Invoke("SetTableReference", objArray)[0];
		}
	}
}