using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace smartRestaurant.CheckBillService
{
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[WebServiceBinding(Name="CheckBillServiceSoap", Namespace="http://ws.smartRestaurant.net")]
	public class CheckBillService : SoapHttpClientProtocol
	{
		public CheckBillService()
		{
			string item = ConfigurationSettings.AppSettings["smartRestaurant.CheckBillService.CheckBillService"];
			if (item == null)
			{
				base.Url = "http://localhost/smartService/CheckBillService.asmx";
				return;
			}
			base.Url = string.Concat(item, "");
		}

		public IAsyncResult BeginGetComputeBillPrice(int orderBillID, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { orderBillID };
			return base.BeginInvoke("GetComputeBillPrice", objArray, callback, asyncState);
		}

		public IAsyncResult BeginGetDescription(string id, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { id };
			return base.BeginInvoke("GetDescription", objArray, callback, asyncState);
		}

		public IAsyncResult BeginGetInvoice(int orderBillID, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { orderBillID };
			return base.BeginInvoke("GetInvoice", objArray, callback, asyncState);
		}

		public IAsyncResult BeginGetInvoiceByID(int invoiceID, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { invoiceID };
			return base.BeginInvoke("GetInvoiceByID", objArray, callback, asyncState);
		}

		public IAsyncResult BeginGetTodayTip(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("GetTodayTip", new object[0], callback, asyncState);
		}

		public IAsyncResult BeginSendInvoice(Invoice invoice, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { invoice };
			return base.BeginInvoke("SendInvoice", objArray, callback, asyncState);
		}

		public BillPrice EndGetComputeBillPrice(IAsyncResult asyncResult)
		{
			return (BillPrice)base.EndInvoke(asyncResult)[0];
		}

		public string EndGetDescription(IAsyncResult asyncResult)
		{
			return (string)base.EndInvoke(asyncResult)[0];
		}

		public Invoice EndGetInvoice(IAsyncResult asyncResult)
		{
			return (Invoice)base.EndInvoke(asyncResult)[0];
		}

		public Invoice EndGetInvoiceByID(IAsyncResult asyncResult)
		{
			return (Invoice)base.EndInvoke(asyncResult)[0];
		}

		public double EndGetTodayTip(IAsyncResult asyncResult)
		{
			return (double)base.EndInvoke(asyncResult)[0];
		}

		public string EndSendInvoice(IAsyncResult asyncResult)
		{
			return (string)base.EndInvoke(asyncResult)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetComputeBillPrice", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public BillPrice GetComputeBillPrice(int orderBillID)
		{
			object[] objArray = new object[] { orderBillID };
			return (BillPrice)base.Invoke("GetComputeBillPrice", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetDescription", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetDescription(string id)
		{
			object[] objArray = new object[] { id };
			return (string)base.Invoke("GetDescription", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetInvoice", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public Invoice GetInvoice(int orderBillID)
		{
			object[] objArray = new object[] { orderBillID };
			return (Invoice)base.Invoke("GetInvoice", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetInvoiceByID", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public Invoice GetInvoiceByID(int invoiceID)
		{
			object[] objArray = new object[] { invoiceID };
			return (Invoice)base.Invoke("GetInvoiceByID", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetTodayTip", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public double GetTodayTip()
		{
			object[] objArray = base.Invoke("GetTodayTip", new object[0]);
			return (double)objArray[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/SendInvoice", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string SendInvoice(Invoice invoice)
		{
			object[] objArray = new object[] { invoice };
			return (string)base.Invoke("SendInvoice", objArray)[0];
		}
	}
}