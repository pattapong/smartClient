using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace smartRestaurant.CustomerService
{
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[WebServiceBinding(Name="CustomerServiceSoap", Namespace="http://ws.smartRestaurant.net")]
	public class CustomerService : SoapHttpClientProtocol
	{
		public CustomerService()
		{
			string item = ConfigurationSettings.AppSettings["smartRestaurant.CustomerService.CustomerService"];
			if (item == null)
			{
				base.Url = "http://localhost/smartService/CustomerService.asmx";
				return;
			}
			base.Url = string.Concat(item, "");
		}

		public IAsyncResult BeginDeleteCustomer(int custID, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { custID };
			return base.BeginInvoke("DeleteCustomer", objArray, callback, asyncState);
		}

		public IAsyncResult BeginGetCustomers(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("GetCustomers", new object[0], callback, asyncState);
		}

		public IAsyncResult BeginGetRoads(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("GetRoads", new object[0], callback, asyncState);
		}

		public IAsyncResult BeginSearchCustomers(string telephone, string fname, string mname, string lname, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { telephone, fname, mname, lname };
			return base.BeginInvoke("SearchCustomers", objArray, callback, asyncState);
		}

		public IAsyncResult BeginSetCustomer(CustomerInformation custInfo, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { custInfo };
			return base.BeginInvoke("SetCustomer", objArray, callback, asyncState);
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/DeleteCustomer", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string DeleteCustomer(int custID)
		{
			object[] objArray = new object[] { custID };
			return (string)base.Invoke("DeleteCustomer", objArray)[0];
		}

		public string EndDeleteCustomer(IAsyncResult asyncResult)
		{
			return (string)base.EndInvoke(asyncResult)[0];
		}

		public CustomerInformation[] EndGetCustomers(IAsyncResult asyncResult)
		{
			return (CustomerInformation[])base.EndInvoke(asyncResult)[0];
		}

		public Road[] EndGetRoads(IAsyncResult asyncResult)
		{
			return (Road[])base.EndInvoke(asyncResult)[0];
		}

		public CustomerInformation[] EndSearchCustomers(IAsyncResult asyncResult)
		{
			return (CustomerInformation[])base.EndInvoke(asyncResult)[0];
		}

		public string EndSetCustomer(IAsyncResult asyncResult)
		{
			return (string)base.EndInvoke(asyncResult)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetCustomers", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public CustomerInformation[] GetCustomers()
		{
			object[] objArray = base.Invoke("GetCustomers", new object[0]);
			return (CustomerInformation[])objArray[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetRoads", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public Road[] GetRoads()
		{
			object[] objArray = base.Invoke("GetRoads", new object[0]);
			return (Road[])objArray[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/SearchCustomers", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public CustomerInformation[] SearchCustomers(string telephone, string fname, string mname, string lname)
		{
			object[] objArray = new object[] { telephone, fname, mname, lname };
			return (CustomerInformation[])base.Invoke("SearchCustomers", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/SetCustomer", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string SetCustomer(CustomerInformation custInfo)
		{
			object[] objArray = new object[] { custInfo };
			return (string)base.Invoke("SetCustomer", objArray)[0];
		}
	}
}