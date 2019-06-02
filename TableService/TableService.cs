using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace smartRestaurant.TableService
{
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[WebServiceBinding(Name="TableServiceSoap", Namespace="http://ws.smartRestaurant.net")]
	public class TableService : SoapHttpClientProtocol
	{
		public TableService()
		{
			string item = ConfigurationSettings.AppSettings["smartRestaurant.TableService.TableService"];
			if (item == null)
			{
				base.Url = "http://localhost/smartService/TableService.asmx";
				return;
			}
			base.Url = string.Concat(item, "");
		}

		public IAsyncResult BeginGetTableInformation(int tableID, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { tableID };
			return base.BeginInvoke("GetTableInformation", objArray, callback, asyncState);
		}

		public IAsyncResult BeginGetTableList(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("GetTableList", new object[0], callback, asyncState);
		}

		public IAsyncResult BeginGetTableStatus(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("GetTableStatus", new object[0], callback, asyncState);
		}

		public IAsyncResult BeginUpdateTableLockInuse(int tableID, bool lockInUse, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { tableID, lockInUse };
			return base.BeginInvoke("UpdateTableLockInuse", objArray, callback, asyncState);
		}

		public TableInformation EndGetTableInformation(IAsyncResult asyncResult)
		{
			return (TableInformation)base.EndInvoke(asyncResult)[0];
		}

		public TableInformation[] EndGetTableList(IAsyncResult asyncResult)
		{
			return (TableInformation[])base.EndInvoke(asyncResult)[0];
		}

		public TableStatus[] EndGetTableStatus(IAsyncResult asyncResult)
		{
			return (TableStatus[])base.EndInvoke(asyncResult)[0];
		}

		public int EndUpdateTableLockInuse(IAsyncResult asyncResult)
		{
			return (int)base.EndInvoke(asyncResult)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetTableInformation", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public TableInformation GetTableInformation(int tableID)
		{
			object[] objArray = new object[] { tableID };
			return (TableInformation)base.Invoke("GetTableInformation", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetTableList", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public TableInformation[] GetTableList()
		{
			object[] objArray = base.Invoke("GetTableList", new object[0]);
			return (TableInformation[])objArray[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetTableStatus", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public TableStatus[] GetTableStatus()
		{
			object[] objArray = base.Invoke("GetTableStatus", new object[0]);
			return (TableStatus[])objArray[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/UpdateTableLockInuse", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public int UpdateTableLockInuse(int tableID, bool lockInUse)
		{
			object[] objArray = new object[] { tableID, lockInUse };
			return (int)base.Invoke("UpdateTableLockInuse", objArray)[0];
		}
	}
}