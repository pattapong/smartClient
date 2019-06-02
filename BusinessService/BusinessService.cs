using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

namespace smartRestaurant.BusinessService
{
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[WebServiceBinding(Name="BusinessServiceSoap", Namespace="http://ws.smartRestaurant.net")]
	public class BusinessService : SoapHttpClientProtocol
	{
		public BusinessService()
		{
			string item = ConfigurationSettings.AppSettings["smartRestaurant.BusinessService.BusinessService"];
			if (item == null)
			{
				base.Url = "http://localhost/smartService/BusinessService.asmx";
				return;
			}
			base.Url = string.Concat(item, "");
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/BackupDatabase", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public void BackupDatabase()
		{
			base.Invoke("BackupDatabase", new object[0]);
		}

		public IAsyncResult BeginBackupDatabase(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("BackupDatabase", new object[0], callback, asyncState);
		}

		public IAsyncResult BeginDeleteSelected(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("DeleteSelected", new object[0], callback, asyncState);
		}

		public IAsyncResult BeginExportInvoice(DateTime fromDate, DateTime toDate, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { fromDate, toDate };
			return base.BeginInvoke("ExportInvoice", objArray, callback, asyncState);
		}

		public IAsyncResult BeginGetBillPrinter(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("GetBillPrinter", new object[0], callback, asyncState);
		}

		public IAsyncResult BeginGetInstalledPrinter(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("GetInstalledPrinter", new object[0], callback, asyncState);
		}

		public IAsyncResult BeginGetInvoiceReport(DateTime date, int empType, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { date, empType };
			return base.BeginInvoke("GetInvoiceReport", objArray, callback, asyncState);
		}

		public IAsyncResult BeginGetInvoiceSummaryReport(DateTime date, int empType, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { date, empType };
			return base.BeginInvoke("GetInvoiceSummaryReport", objArray, callback, asyncState);
		}

		public IAsyncResult BeginGetKitchenPrinter(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("GetKitchenPrinter", new object[0], callback, asyncState);
		}

		public IAsyncResult BeginGetReceiptPrinter(AsyncCallback callback, object asyncState)
		{
			return base.BeginInvoke("GetReceiptPrinter", new object[0], callback, asyncState);
		}

		public IAsyncResult BeginGetSalesReport(DateTime date, int empType, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { date, empType };
			return base.BeginInvoke("GetSalesReport", objArray, callback, asyncState);
		}

		public IAsyncResult BeginPrintInvoiceReport(DateTime date, int empType, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { date, empType };
			return base.BeginInvoke("PrintInvoiceReport", objArray, callback, asyncState);
		}

		public IAsyncResult BeginPrintSummaryMenuType(DateTime date, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { date };
			return base.BeginInvoke("PrintSummaryMenuType", objArray, callback, asyncState);
		}

		public IAsyncResult BeginPrintSummaryReceive(DateTime date, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { date };
			return base.BeginInvoke("PrintSummaryReceive", objArray, callback, asyncState);
		}

		public IAsyncResult BeginPrintTaxSummary(int month, int year, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { month, year };
			return base.BeginInvoke("PrintTaxSummary", objArray, callback, asyncState);
		}

		public IAsyncResult BeginSetBillPrinter(string printerName, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { printerName };
			return base.BeginInvoke("SetBillPrinter", objArray, callback, asyncState);
		}

		public IAsyncResult BeginSetKitchenPrinter(string printerName, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { printerName };
			return base.BeginInvoke("SetKitchenPrinter", objArray, callback, asyncState);
		}

		public IAsyncResult BeginSetReceiptPrinter(string printerName, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { printerName };
			return base.BeginInvoke("SetReceiptPrinter", objArray, callback, asyncState);
		}

		public IAsyncResult BeginUpdateInvoiceHidden(int invoiceID, bool hidden, AsyncCallback callback, object asyncState)
		{
			object[] objArray = new object[] { invoiceID, hidden };
			return base.BeginInvoke("UpdateInvoiceHidden", objArray, callback, asyncState);
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/DeleteSelected", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public void DeleteSelected()
		{
			base.Invoke("DeleteSelected", new object[0]);
		}

		public void EndBackupDatabase(IAsyncResult asyncResult)
		{
			base.EndInvoke(asyncResult);
		}

		public void EndDeleteSelected(IAsyncResult asyncResult)
		{
			base.EndInvoke(asyncResult);
		}

		public DataStream[] EndExportInvoice(IAsyncResult asyncResult)
		{
			return (DataStream[])base.EndInvoke(asyncResult)[0];
		}

		public string EndGetBillPrinter(IAsyncResult asyncResult)
		{
			return (string)base.EndInvoke(asyncResult)[0];
		}

		public string[] EndGetInstalledPrinter(IAsyncResult asyncResult)
		{
			return (string[])base.EndInvoke(asyncResult)[0];
		}

		public DataStream[] EndGetInvoiceReport(IAsyncResult asyncResult)
		{
			return (DataStream[])base.EndInvoke(asyncResult)[0];
		}

		public DataStream[] EndGetInvoiceSummaryReport(IAsyncResult asyncResult)
		{
			return (DataStream[])base.EndInvoke(asyncResult)[0];
		}

		public string EndGetKitchenPrinter(IAsyncResult asyncResult)
		{
			return (string)base.EndInvoke(asyncResult)[0];
		}

		public string EndGetReceiptPrinter(IAsyncResult asyncResult)
		{
			return (string)base.EndInvoke(asyncResult)[0];
		}

		public DataStream[] EndGetSalesReport(IAsyncResult asyncResult)
		{
			return (DataStream[])base.EndInvoke(asyncResult)[0];
		}

		public void EndPrintInvoiceReport(IAsyncResult asyncResult)
		{
			base.EndInvoke(asyncResult);
		}

		public void EndPrintSummaryMenuType(IAsyncResult asyncResult)
		{
			base.EndInvoke(asyncResult);
		}

		public void EndPrintSummaryReceive(IAsyncResult asyncResult)
		{
			base.EndInvoke(asyncResult);
		}

		public void EndPrintTaxSummary(IAsyncResult asyncResult)
		{
			base.EndInvoke(asyncResult);
		}

		public void EndSetBillPrinter(IAsyncResult asyncResult)
		{
			base.EndInvoke(asyncResult);
		}

		public void EndSetKitchenPrinter(IAsyncResult asyncResult)
		{
			base.EndInvoke(asyncResult);
		}

		public void EndSetReceiptPrinter(IAsyncResult asyncResult)
		{
			base.EndInvoke(asyncResult);
		}

		public void EndUpdateInvoiceHidden(IAsyncResult asyncResult)
		{
			base.EndInvoke(asyncResult);
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/ExportInvoice", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public DataStream[] ExportInvoice(DateTime fromDate, DateTime toDate)
		{
			object[] objArray = new object[] { fromDate, toDate };
			return (DataStream[])base.Invoke("ExportInvoice", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetBillPrinter", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetBillPrinter()
		{
			object[] objArray = base.Invoke("GetBillPrinter", new object[0]);
			return (string)objArray[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetInstalledPrinter", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string[] GetInstalledPrinter()
		{
			object[] objArray = base.Invoke("GetInstalledPrinter", new object[0]);
			return (string[])objArray[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetInvoiceReport", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public DataStream[] GetInvoiceReport(DateTime date, int empType)
		{
			object[] objArray = new object[] { date, empType };
			return (DataStream[])base.Invoke("GetInvoiceReport", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetInvoiceSummaryReport", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public DataStream[] GetInvoiceSummaryReport(DateTime date, int empType)
		{
			object[] objArray = new object[] { date, empType };
			return (DataStream[])base.Invoke("GetInvoiceSummaryReport", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetKitchenPrinter", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetKitchenPrinter()
		{
			object[] objArray = base.Invoke("GetKitchenPrinter", new object[0]);
			return (string)objArray[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetReceiptPrinter", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public string GetReceiptPrinter()
		{
			object[] objArray = base.Invoke("GetReceiptPrinter", new object[0]);
			return (string)objArray[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/GetSalesReport", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public DataStream[] GetSalesReport(DateTime date, int empType)
		{
			object[] objArray = new object[] { date, empType };
			return (DataStream[])base.Invoke("GetSalesReport", objArray)[0];
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/PrintInvoiceReport", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public void PrintInvoiceReport(DateTime date, int empType)
		{
			object[] objArray = new object[] { date, empType };
			base.Invoke("PrintInvoiceReport", objArray);
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/PrintSummaryMenuType", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public void PrintSummaryMenuType(DateTime date)
		{
			object[] objArray = new object[] { date };
			base.Invoke("PrintSummaryMenuType", objArray);
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/PrintSummaryReceive", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public void PrintSummaryReceive(DateTime date)
		{
			object[] objArray = new object[] { date };
			base.Invoke("PrintSummaryReceive", objArray);
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/PrintTaxSummary", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public void PrintTaxSummary(int month, int year)
		{
			object[] objArray = new object[] { month, year };
			base.Invoke("PrintTaxSummary", objArray);
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/SetBillPrinter", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public void SetBillPrinter(string printerName)
		{
			object[] objArray = new object[] { printerName };
			base.Invoke("SetBillPrinter", objArray);
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/SetKitchenPrinter", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public void SetKitchenPrinter(string printerName)
		{
			object[] objArray = new object[] { printerName };
			base.Invoke("SetKitchenPrinter", objArray);
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/SetReceiptPrinter", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public void SetReceiptPrinter(string printerName)
		{
			object[] objArray = new object[] { printerName };
			base.Invoke("SetReceiptPrinter", objArray);
		}

		[SoapDocumentMethod("http://ws.smartRestaurant.net/UpdateInvoiceHidden", RequestNamespace="http://ws.smartRestaurant.net", ResponseNamespace="http://ws.smartRestaurant.net", Use=SoapBindingUse.Literal, ParameterStyle=SoapParameterStyle.Wrapped)]
		public void UpdateInvoiceHidden(int invoiceID, bool hidden)
		{
			object[] objArray = new object[] { invoiceID, hidden };
			base.Invoke("UpdateInvoiceHidden", objArray);
		}
	}
}