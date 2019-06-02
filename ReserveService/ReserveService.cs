using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace smartRestaurant.ReserveService
{
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[WebServiceBinding(Name="ReserveServiceSoap", Namespace="http://ws.smartRestaurant.net")]
	public class ReserveService : SoapHttpClientProtocol
	{
		public ReserveService()
		{
			string item = ConfigurationSettings.AppSettings["smartRestaurant.ReserveService.ReserveService"];
			if (item == null)
			{
				base.Url = "http://localhost/smartService/ReserveService.asmx";
				return;
			}
			base.Url = string.Concat(item, "");
		}

		public IAsyncResult BeginGetTableReserve(DateTime date, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { date };
			return base.BeginInvoke("GetTableReserve", objArray, callback, asyncState);
		}

		public IAsyncResult BeginSetReserveCancel(string reserveID, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { reserveID };
			return base.BeginInvoke("SetReserveCancel", objArray, callback, asyncState);
		}

		public IAsyncResult BeginSetReserveDinIn(string reserveID, string TableID, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { reserveID, TableID };
			return base.BeginInvoke("SetReserveDinIn", objArray, callback, asyncState);
		}

		public IAsyncResult BeginSetTableReserve(TableReserve reserve, string custFullName, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { reserve, custFullName };
			return base.BeginInvoke("SetTableReserve", objArray, callback, asyncState);
		}

		public TableReserve[] EndGetTableReserve(IAsyncResult asyncResult)
		{
			return (TableReserve[])base.EndInvoke(asyncResult)[0];
		}

		public int EndSetReserveCancel(IAsyncResult asyncResult)
		{
			return (int)base.EndInvoke(asyncResult)[0];
		}

		public int EndSetReserveDinIn(IAsyncResult asyncResult)
		{
			return (int)base.EndInvoke(asyncResult)[0];
		}

		public string EndSetTableReserve(IAsyncResult asyncResult)
		{
			return (string)base.EndInvoke(asyncResult)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetTableReserve", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public TableReserve[] GetTableReserve(DateTime date)
		{
			object[] objArray = new object[] { date };
			return (TableReserve[])base.Invoke("GetTableReserve", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/SetReserveCancel", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public int SetReserveCancel(string reserveID)
		{
			object[] objArray = new object[] { reserveID };
			return (int)base.Invoke("SetReserveCancel", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/SetReserveDinIn", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public int SetReserveDinIn(string reserveID, string TableID)
		{
			object[] objArray = new object[] { reserveID, TableID };
			return (int)base.Invoke("SetReserveDinIn", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/SetTableReserve", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string SetTableReserve(TableReserve reserve, string custFullName)
		{
			object[] objArray = new object[] { reserve, custFullName };
			return (string)base.Invoke("SetTableReserve", objArray)[0];
		}
	}
}