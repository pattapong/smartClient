using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace smartRestaurant.UserAuthorizeService
{
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[WebServiceBinding(Name="UserAuthorizeServiceSoap", Namespace="http://ws.smartRestaurant.net")]
	public class UserAuthorizeService : SoapHttpClientProtocol
	{
		public UserAuthorizeService()
		{
			string item = ConfigurationSettings.AppSettings["smartRestaurant.UserAuthorizeService.UserAuthorizeService"];
			if (item == null)
			{
				base.Url = "http://localhost/smartService/UserAuthorizeService.asmx";
				return;
			}
			base.Url = string.Concat(item, "");
		}

		public IAsyncResult BeginCheckLogin(int employeeID, string password, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { employeeID, password };
			return base.BeginInvoke("CheckLogin", objArray, callback, asyncState);
		}

		public IAsyncResult BeginCheckLogout(int employeeID, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { employeeID };
			return base.BeginInvoke("CheckLogout", objArray, callback, asyncState);
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/CheckLogin", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public UserProfile CheckLogin(int employeeID, string password)
		{
			object[] objArray = new object[] { employeeID, password };
			return (UserProfile)base.Invoke("CheckLogin", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/CheckLogout", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string CheckLogout(int employeeID)
		{
			object[] objArray = new object[] { employeeID };
			return (string)base.Invoke("CheckLogout", objArray)[0];
		}

		public UserProfile EndCheckLogin(IAsyncResult asyncResult)
		{
			return (UserProfile)base.EndInvoke(asyncResult)[0];
		}

		public string EndCheckLogout(IAsyncResult asyncResult)
		{
			return (string)base.EndInvoke(asyncResult)[0];
		}
	}
}