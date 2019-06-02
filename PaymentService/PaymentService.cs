using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace smartRestaurant.PaymentService
{
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[WebServiceBinding(Name="PaymentServiceSoap", Namespace="http://ws.smartRestaurant.net")]
	public class PaymentService : SoapHttpClientProtocol
	{
		public PaymentService()
		{
			string item = ConfigurationSettings.AppSettings["smartRestaurant.PaymentService.PaymentService"];
			if (item == null)
			{
				base.Url = "http://localhost/smartService/PaymentService.asmx";
				return;
			}
			base.Url = string.Concat(item, "");
		}

		public IAsyncResult BeginGetCoupons(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("GetCoupons", new object[0], callback, asyncState);
		}

		public IAsyncResult BeginGetDiscounts(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("GetDiscounts", new object[0], callback, asyncState);
		}

		public IAsyncResult BeginGetGiftVouchers(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("GetGiftVouchers", new object[0], callback, asyncState);
		}

		public IAsyncResult BeginGetPaymentMethods(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("GetPaymentMethods", new object[0], callback, asyncState);
		}

		public PromotionCard[] EndGetCoupons(IAsyncResult asyncResult)
		{
			return (PromotionCard[])base.EndInvoke(asyncResult)[0];
		}

		public Discount[] EndGetDiscounts(IAsyncResult asyncResult)
		{
			return (Discount[])base.EndInvoke(asyncResult)[0];
		}

		public PromotionCard[] EndGetGiftVouchers(IAsyncResult asyncResult)
		{
			return (PromotionCard[])base.EndInvoke(asyncResult)[0];
		}

		public PaymentMethod[] EndGetPaymentMethods(IAsyncResult asyncResult)
		{
			return (PaymentMethod[])base.EndInvoke(asyncResult)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetCoupons", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public PromotionCard[] GetCoupons()
		{
			object[] objArray = base.Invoke("GetCoupons", new object[0]);
			return (PromotionCard[])objArray[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetDiscounts", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public Discount[] GetDiscounts()
		{
			object[] objArray = base.Invoke("GetDiscounts", new object[0]);
			return (Discount[])objArray[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetGiftVouchers", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public PromotionCard[] GetGiftVouchers()
		{
			object[] objArray = base.Invoke("GetGiftVouchers", new object[0]);
			return (PromotionCard[])objArray[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetPaymentMethods", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public PaymentMethod[] GetPaymentMethods()
		{
			object[] objArray = base.Invoke("GetPaymentMethods", new object[0]);
			return (PaymentMethod[])objArray[0];
		}
	}
}