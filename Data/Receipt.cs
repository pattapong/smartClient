using System;
using System.Collections;
using System.Windows.Forms;
using smartRestaurant.CheckBillService;
using smartRestaurant.MenuService;
using smartRestaurant.OrderService;
using smartRestaurant.PaymentService;
using smartRestaurant.Utils;
namespace smartRestaurant.Data
{
	/// <summary>
	/// Summary description for Receipt.
	/// </summary>
	public class Receipt
	{
		// Fields
		private double amountDue;
		private double change;
		private PromotionCard coupon;
		private static PromotionCard[] coupons;
		private static Discount[] discounts;
		private int employeeID;
		private PromotionCard giftVoucher;
		private static PromotionCard[] giftVouchers;
		private int invoiceID;
		private string invoiceNote;
		private int orderBillID;
		private PaymentMethod paymentMethod;
		private ArrayList paymentMethodList;
		private static PaymentMethod[] paymentMethods;
		private double payValue;
		private ArrayList payValueList;
		private int pointAmount;
		private OrderBill selectedBill;
		private double tax1;
		private double tax2;
		private double totalDiscount;
		private double totalDue;
		private double totalReceive;
		private ArrayList useDiscount;

		// Methods
		public Receipt(int invoiceID)
		{
			this.selectedBill = null;
			this.totalReceive = 0.0;
			this.paymentMethod = null;
			this.paymentMethodList = new ArrayList();
			this.payValueList = new ArrayList();
			this.coupon = null;
			this.giftVoucher = null;
			this.useDiscount = new ArrayList();
			this.invoiceID = invoiceID;
			this.invoiceNote = string.Empty;
			this.LoadInvoice();
		}

		public Receipt(OrderBill bill, int employeeID)
		{
			this.selectedBill = bill;
			this.totalReceive = 0.0;
			this.paymentMethod = null;
			this.paymentMethodList = new ArrayList();
			this.payValueList = new ArrayList();
			this.coupon = null;
			this.giftVoucher = null;
			this.useDiscount = new ArrayList();
			this.employeeID = employeeID;
			this.invoiceID = 0;
			this.invoiceNote = string.Empty;
			this.LoadInvoice();
		}

		public void ClearPaymentMethod()
		{
			this.paymentMethodList.Clear();
			this.payValueList.Clear();
		}

		public void Compute()
		{
			this.totalDue = 0.0;
			double num = 0.0;
			double num2 = 0.0;
			if (this.useDiscount.Count > 0)
			{
				for (int j = 0; j < this.useDiscount.Count; j++)
				{
					Discount discount = (Discount) this.useDiscount[j];
					if (discount.discountPercent > 0.0)
					{
						num2 += discount.discountPercent;
					}
					else if (discount.discountAmount > 0.0)
					{
						num += discount.discountAmount;
					}
				}
			}
			if ((this.selectedBill != null) && (this.selectedBill.Items != null))
			{
				BillPrice computeBillPrice = new smartRestaurant.CheckBillService.CheckBillService().GetComputeBillPrice(this.selectedBill.OrderBillID);
				this.amountDue = computeBillPrice.totalPrice;
				this.totalDiscount = computeBillPrice.totalDiscount;
				this.tax1 = computeBillPrice.totalTax1;
				this.tax2 = computeBillPrice.totalTax2;
			}
			else if (this.selectedBill != null)
			{
				this.totalDiscount = num;
				this.amountDue = this.tax1 = this.tax2 = 0.0;
			}
			this.totalDue = ((this.amountDue - this.totalDiscount) + this.tax1) + this.tax2;
			double num4 = 0.0;
			for (int i = 0; i < this.payValueList.Count; i++)
			{
				num4 += (double) this.payValueList[i];
			}
			this.totalReceive = ((num4 + this.PointValue) + this.CouponValue) + this.GiftVoucherValue;
			if (this.totalDue < 0.0)
			{
				this.totalDue = 0.0;
			}
			this.change = this.totalReceive - this.totalDue;
			if (this.change < 0.0)
			{
				this.change = 0.0;
			}
		}

		private Invoice CreateInvoice()
		{
			Invoice invoice = new Invoice();
			invoice.invoiceID = this.invoiceID;
			if (this.paymentMethod != null)
			{
				invoice.paymentMethodID = this.paymentMethod.paymentMethodID;
			}
			else
			{
				invoice.paymentMethodID = PaymentMethods[0].paymentMethodID;
			}
			if (this.selectedBill != null)
			{
				invoice.orderBillID = this.selectedBill.OrderBillID;
			}
			else
			{
				invoice.orderBillID = this.orderBillID;
			}
			invoice.point = this.pointAmount;
			invoice.totalPrice = this.amountDue;
			invoice.tax1 = this.tax1;
			invoice.tax2 = this.tax2;
			invoice.totalDiscount = this.totalDiscount;
			invoice.totalReceive = this.totalReceive;
			invoice.employeeID = this.employeeID;
			invoice.refInvoice = 0;
			invoice.invoiceNote = this.invoiceNote;
			invoice.discounts = null;
			if (this.useDiscount.Count > 0)
			{
				invoice.discounts = new InvoiceDiscount[this.useDiscount.Count];
				for (int i = 0; i < this.useDiscount.Count; i++)
				{
					invoice.discounts[i] = new InvoiceDiscount();
					invoice.discounts[i].promotionID = ((Discount) this.useDiscount[i]).promotionID;
				}
			}
			invoice.payments = null;
			if (this.paymentMethodList.Count > 0)
			{
				invoice.payments = new InvoicePayment[this.paymentMethodList.Count];
				for (int j = 0; j < this.paymentMethodList.Count; j++)
				{
					invoice.payments[j] = new InvoicePayment();
					invoice.payments[j].paymentMethodID = ((PaymentMethod) this.paymentMethodList[j]).paymentMethodID;
					invoice.payments[j].receive = (double) this.payValueList[j];
				}
			}
			return invoice;
		}

		private void LoadInvoice()
		{
			Invoice invoiceByID;
			smartRestaurant.CheckBillService.CheckBillService service = new smartRestaurant.CheckBillService.CheckBillService();
			if (this.invoiceID == 0)
			{
				invoiceByID = service.GetInvoice(this.selectedBill.OrderBillID);
			}
			else
			{
				invoiceByID = service.GetInvoiceByID(this.invoiceID);
			}
			if (invoiceByID == null)
			{
				this.SendInvoice(false, false);
			}
			else if (invoiceByID.invoiceID == 0)
			{
				MessageBox.Show("Can't load invoice data");
			}
			else
			{
				this.invoiceID = invoiceByID.invoiceID;
				this.invoiceNote = invoiceByID.invoiceNote;
				this.paymentMethod = null;
				if (PaymentMethods != null)
				{
					for (int i = 0; i < PaymentMethods.Length; i++)
					{
						if (PaymentMethods[i].paymentMethodID == invoiceByID.paymentMethodID)
						{
							this.paymentMethod = PaymentMethods[i];
							break;
						}
					}
				}
				if (this.selectedBill == null)
				{
					this.employeeID = invoiceByID.employeeID;
					this.orderBillID = invoiceByID.orderBillID;
				}
				this.amountDue = invoiceByID.totalPrice;
				this.pointAmount = invoiceByID.point;
				this.tax1 = invoiceByID.tax1;
				this.tax2 = invoiceByID.tax2;
				this.totalDiscount = invoiceByID.totalDiscount;
				this.totalReceive = invoiceByID.totalReceive;
				this.totalDue = ((this.amountDue + this.tax1) + this.tax2) - this.totalDiscount;
				this.useDiscount.Clear();
				if (invoiceByID.discounts != null)
				{
					for (int j = 0; j < invoiceByID.discounts.Length; j++)
					{
						for (int k = 0; k < Discounts.Length; k++)
						{
							if (Discounts[k].promotionID == invoiceByID.discounts[j].promotionID)
							{
								this.useDiscount.Add(Discounts[k]);
								break;
							}
						}
					}
				}
				this.paymentMethodList.Clear();
				this.payValueList.Clear();
				if (invoiceByID.payments != null)
				{
					for (int m = 0; m < invoiceByID.payments.Length; m++)
					{
						for (int n = 0; n < PaymentMethods.Length; n++)
						{
							if (PaymentMethods[n].paymentMethodID == invoiceByID.payments[m].paymentMethodID)
							{
								this.paymentMethodList.Add(PaymentMethods[n]);
								this.payValueList.Add(invoiceByID.payments[m].receive);
								break;
							}
						}
					}
				}
			}
		}

		public void PrintInvoice()
		{
			Invoice invoice = this.CreateInvoice();
			new smartRestaurant.OrderService.OrderService().ReprintBill(invoice.orderBillID);
		}

		public static void PrintReceiptAll(OrderInformation orderInfo)
		{
			smartRestaurant.OrderService.OrderService service = new smartRestaurant.OrderService.OrderService();
			for (int i = 0; i < orderInfo.Bills.Length; i++)
			{
				if (((orderInfo.Bills[i].Items != null) && (orderInfo.Bills[i].Items.Length > 0)) && (orderInfo.Bills[i].CloseBillDate == AppParameter.MinDateTime))
				{
					service.PrintReceipt(orderInfo.Bills[i].OrderBillID);
				}
			}
		}

		public static Discount SearchDiscountByID(int id)
		{
			for (int i = 0; i < discounts.Length; i++)
			{
				if (discounts[i].promotionID == id)
				{
					return discounts[i];
				}
			}
			return null;
		}

		public static PaymentMethod SearchPaymentMethodByID(int id)
		{
			for (int i = 0; i < paymentMethods.Length; i++)
			{
				if (paymentMethods[i].paymentMethodID == id)
				{
					return paymentMethods[i];
				}
			}
			return null;
		}

		public bool SendInvoice(bool closed, bool print)
		{
			smartRestaurant.CheckBillService.CheckBillService service = new smartRestaurant.CheckBillService.CheckBillService();
			Invoice invoice = this.CreateInvoice();
			if (!closed)
			{
				invoice.totalReceive = 0.0;
			}
			string s = service.SendInvoice(invoice);
			try
			{
				this.invoiceID = int.Parse(s);
			}
			catch (Exception)
			{
				MessageBox.Show(s);
				return true;
			}
			if (!closed && (this.invoiceID == -1))
			{
				return false;
			}
			if (print)
			{
				smartRestaurant.OrderService.OrderService service2 = new smartRestaurant.OrderService.OrderService();
				if (closed)
				{
					service2.PrintBill(invoice.orderBillID);
				}
				else
				{
					service2.PrintReceipt(invoice.orderBillID);
				}
			}
			if (this.invoiceID == -1)
			{
				return false;
			}
			return true;
		}

		public void SetPaymentMethod(PaymentMethod method, double val)
		{
			if (method == null)
			{
				this.paymentMethodList.Clear();
				this.payValueList.Clear();
			}
			else
			{
				int index = this.paymentMethodList.IndexOf(method);
				if (val != 0.0)
				{
					if (index >= 0)
					{
						this.payValueList[index] = val;
					}
					else
					{
						this.paymentMethodList.Add(method);
						this.payValueList.Add(val);
					}
				}
				else if (index >= 0)
				{
					this.paymentMethodList.RemoveAt(index);
					this.payValueList.RemoveAt(index);
				}
			}
			this.payValue = 0.0;
			for (int i = 0; i < this.payValueList.Count; i++)
			{
				this.payValue += (double) this.payValueList[i];
			}
			this.totalReceive = this.payValue;
			this.paymentMethod = null;
		}

		public void UseDiscountAdd(Discount dis)
		{
			this.UseDiscount.Add(dis);
			this.SendInvoice(false, false);
		}

		public void UseDiscountRemove(Discount dis)
		{
			this.UseDiscount.Remove(dis);
			this.SendInvoice(false, false);
		}

		// Properties
		public double AmountDue
		{
			get
			{
				return this.amountDue;
			}
		}

		public double Change
		{
			get
			{
				return this.change;
			}
		}

		public PromotionCard Coupon
		{
			get
			{
				return this.coupon;
			}
			set
			{
				this.coupon = value;
			}
		}

		public static PromotionCard[] Coupons
		{
			get
			{
				if (coupons == null)
				{
					coupons = new smartRestaurant.PaymentService.PaymentService().GetCoupons();
				}
				return coupons;
			}
		}

		public double CouponValue
		{
			get
			{
				if (this.coupon != null)
				{
					return this.coupon.amount;
				}
				return 0.0;
			}
		}

		public static Discount[] Discounts
		{
			get
			{
				if (discounts == null)
				{
					discounts = new smartRestaurant.PaymentService.PaymentService().GetDiscounts();
				}
				return discounts;
			}
		}

		public PromotionCard GiftVoucher
		{
			get
			{
				return this.giftVoucher;
			}
			set
			{
				this.giftVoucher = value;
			}
		}

		public static PromotionCard[] GiftVouchers
		{
			get
			{
				if (giftVouchers == null)
				{
					giftVouchers = new smartRestaurant.PaymentService.PaymentService().GetGiftVouchers();
				}
				return giftVouchers;
			}
		}

		public double GiftVoucherValue
		{
			get
			{
				if (this.giftVoucher != null)
				{
					return this.giftVoucher.amount;
				}
				return 0.0;
			}
		}

		public string InvoiceNote
		{
			get
			{
				return this.invoiceNote;
			}
			set
			{
				this.invoiceNote = value;
			}
		}

		public bool IsCompleted
		{
			get
			{
				return (((long) (this.totalReceive * 100.0)) >= ((long) (this.totalDue * 100.0)));
			}
		}

		public PaymentMethod PaymentMethod
		{
			get
			{
				return this.paymentMethod;
			}
			set
			{
				this.paymentMethod = value;
			}
		}

		public ArrayList PaymentMethodList
		{
			get
			{
				return this.paymentMethodList;
			}
			set
			{
				this.paymentMethodList = value;
			}
		}

		public static PaymentMethod[] PaymentMethods
		{
			get
			{
				if (paymentMethods == null)
				{
					paymentMethods = new smartRestaurant.PaymentService.PaymentService().GetPaymentMethods();
				}
				return paymentMethods;
			}
		}

		public double PayValue
		{
			get
			{
				return this.payValue;
			}
			set
			{
				this.payValue = value;
			}
		}

		public ArrayList PayValueList
		{
			get
			{
				return this.payValueList;
			}
			set
			{
				this.payValueList = value;
			}
		}

		public int PointAmount
		{
			get
			{
				return this.pointAmount;
			}
			set
			{
				this.pointAmount = value;
			}
		}

		public double PointValue
		{
			get
			{
				return (((double) this.pointAmount) / 100.0);
			}
		}

		public double Tax1
		{
			get
			{
				return this.tax1;
			}
		}

		public double Tax2
		{
			get
			{
				return this.tax2;
			}
		}

		public double TotalDiscount
		{
			get
			{
				return this.totalDiscount;
			}
		}

		public double TotalDue
		{
			get
			{
				return this.totalDue;
			}
		}

		public double TotalReceive
		{
			get
			{
				return this.totalReceive;
			}
			set
			{
				this.totalReceive = value;
			}
		}

		public ArrayList UseDiscount
		{
			get
			{
				return this.useDiscount;
			}
		}
	}


}
