using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace smartRestaurant.MenuService
{
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[WebServiceBinding(Name="MenuServiceSoap", Namespace="http://ws.smartRestaurant.net")]
	public class MenuService : SoapHttpClientProtocol
	{
		public MenuService()
		{
			string item = ConfigurationSettings.AppSettings["smartRestaurant.MenuService.MenuService"];
			if (item == null)
			{
				base.Url = "http://localhost/smartService/MenuService.asmx";
				return;
			}
			base.Url = string.Concat(item, "");
		}

		public IAsyncResult BeginGetMenus(string AppName, AsyncCallback callback, object asyncState)
		{
			object[] appName = new object[] { AppName };
			return base.BeginInvoke("GetMenus", appName, callback, asyncState);
		}

		public IAsyncResult BeginGetOptions(string AppName, AsyncCallback callback, object asyncState)
		{
			object[] appName = new object[] { AppName };
			return base.BeginInvoke("GetOptions", appName, callback, asyncState);
		}

		public MenuType[] EndGetMenus(IAsyncResult asyncResult)
		{
			return (MenuType[])base.EndInvoke(asyncResult)[0];
		}

		public MenuOption[] EndGetOptions(IAsyncResult asyncResult)
		{
			return (MenuOption[])base.EndInvoke(asyncResult)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetMenus", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public MenuType[] GetMenus(string AppName)
		{
			object[] appName = new object[] { AppName };
			return (MenuType[])base.Invoke("GetMenus", appName)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetOptions", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public MenuOption[] GetOptions(string AppName)
		{
			object[] appName = new object[] { AppName };
			return (MenuOption[])base.Invoke("GetOptions", appName)[0];
		}
	}
}