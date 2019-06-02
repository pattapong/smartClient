using smartRestaurant.CheckBillService;
using smartRestaurant.OrderService;
using smartRestaurant.PaymentService;
using smartRestaurant.Utils;
using System;
using System.Collections;
using System.Windows.Forms;

namespace smartRestaurant.Data
{
	public class Receipt
	{
		private static smartRestaurant.PaymentService.PaymentMethod[] paymentMethods;

		private static PromotionCard[] coupons;

		private static PromotionCard[] giftVouchers;

		private static Discount[] discounts;

		private OrderBill selectedBill;

		private int employeeID;

		private int invoiceID;

		private int orderBillID;

		private string invoiceNote;

		private double amountDue;

		private double tax1;

		private double tax2;

		private double totalDiscount;

		private double totalDue;

		private double totalReceive;

		private double change;

		private ArrayList useDiscount;

		private smartRestaurant.PaymentService.PaymentMethod paymentMethod;

		private double payValue;

		private ArrayList paymentMethodList;

		private ArrayList payValueList;

		private int pointAmount;

		private PromotionCard coupon;

		private PromotionCard giftVoucher;

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
				if (Receipt.coupons == null)
				{
					Receipt.coupons = (new smartRestaurant.PaymentService.PaymentService()).GetCoupons();
				}
				return Receipt.coupons;
			}
		}

		public double CouponValue
		{
			get
			{
				if (this.coupon == null)
				{
					return 0;
				}
				return this.coupon.amount;
			}
		}

		public static Discount[] Discounts
		{
			get
			{
				if (Receipt.discounts == null)
				{
					Receipt.discounts = (new smartRestaurant.PaymentService.PaymentService()).GetDiscounts();
				}
				return Receipt.discounts;
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
				if (Receipt.giftVouchers == null)
				{
					Receipt.giftVouchers = (new smartRestaurant.PaymentService.PaymentService()).GetGiftVouchers();
				}
				return Receipt.giftVouchers;
			}
		}

		public double GiftVoucherValue
		{
			get
			{
				if (this.giftVoucher == null)
				{
					return 0;
				}
				return this.giftVoucher.amount;
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
				return (long)(this.totalReceive * 100) >= (long)(this.totalDue * 100);
			}
		}

		public smartRestaurant.PaymentService.PaymentMethod PaymentMethod
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

		public static smartRestaurant.PaymentService.PaymentMethod[] PaymentMethods
		{
			get
			{
				if (Receipt.paymentMethods == null)
				{
					Receipt.paymentMethods = (new smartRestaurant.PaymentService.PaymentService()).GetPaymentMethods();
				}
				return Receipt.paymentMethods;
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
				return (double)this.pointAmount / 100;
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

		public Receipt(OrderBill bill, int employeeID)
		{
			this.selectedBill = bill;
			this.totalReceive = 0;
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

		public Receipt(int invoiceID)
		{
			this.selectedBill = null;
			this.totalReceive = 0;
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

		public void ClearPaymentMethod()
		{
			this.paymentMethodList.Clear();
			this.payValueList.Clear();
		}

		public void Compute()
		{
			this.totalDue = 0;
			double num = 0;
			double num1 = 0;
			if (this.useDiscount.Count > 0)
			{
				for (int i = 0; i < this.useDiscount.Count; i++)
				{
					Discount item = (Discount)this.useDiscount[i];
					if (item.discountPercent > 0)
					{
						num1 += item.discountPercent;
					}
					else if (item.discountAmount > 0)
					{
						num += item.discountAmount;
					}
				}
			}
			if (this.selectedBill != null && this.selectedBill.Items != null)
			{
				BillPrice computeBillPrice = (new smartRestaurant.CheckBillService.CheckBillService()).GetComputeBillPrice(this.selectedBill.OrderBillID);
				this.amountDue = computeBillPrice.totalPrice;
				this.totalDiscount = computeBillPrice.totalDiscount;
				this.tax1 = computeBillPrice.totalTax1;
				this.tax2 = computeBillPrice.totalTax2;
			}
			else if (this.selectedBill != null)
			{
				this.totalDiscount = num;
				double num2 = 0;
				double num3 = num2;
				this.tax2 = num2;
				double num4 = num3;
				num3 = num4;
				this.tax1 = num4;
				this.amountDue = num3;
			}
			this.totalDue = this.amountDue - this.totalDiscount + this.tax1 + this.tax2;
			double item1 = 0;
			for (int j = 0; j < this.payValueList.Count; j++)
			{
				item1 += (double)this.payValueList[j];
			}
			this.totalReceive = item1 + this.PointValue + this.CouponValue + this.GiftVoucherValue;
			if (this.totalDue < 0)
			{
				this.totalDue = 0;
			}
			this.change = this.totalReceive - this.totalDue;
			if (this.change < 0)
			{
				this.change = 0;
			}
		}

		private Invoice CreateInvoice()
		{
			Invoice invoice = new Invoice()
			{
				invoiceID = this.invoiceID
			};
			if (this.paymentMethod == null)
			{
				invoice.paymentMethodID = Receipt.PaymentMethods[0].paymentMethodID;
			}
			else
			{
				invoice.paymentMethodID = this.paymentMethod.paymentMethodID;
			}
			if (this.selectedBill == null)
			{
				invoice.orderBillID = this.orderBillID;
			}
			else
			{
				invoice.orderBillID = this.selectedBill.OrderBillID;
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
					invoice.discounts[i].promotionID = ((Discount)this.useDiscount[i]).promotionID;
				}
			}
			invoice.payments = null;
			if (this.paymentMethodList.Count > 0)
			{
				invoice.payments = new InvoicePayment[this.paymentMethodList.Count];
				for (int j = 0; j < this.paymentMethodList.Count; j++)
				{
					invoice.payments[j] = new InvoicePayment();
					invoice.payments[j].paymentMethodID = ((smartRestaurant.PaymentService.PaymentMethod)this.paymentMethodList[j]).paymentMethodID;
					invoice.payments[j].receive = (double)this.payValueList[j];
				}
			}
			return invoice;
		}

		private void LoadInvoice()
		{
			Invoice invoice;
			smartRestaurant.CheckBillService.CheckBillService checkBillService = new smartRestaurant.CheckBillService.CheckBillService();
			invoice = (this.invoiceID != 0 ? checkBillService.GetInvoiceByID(this.invoiceID) : checkBillService.GetInvoice(this.selectedBill.OrderBillID));
			if (invoice == null)
			{
				this.SendInvoice(false, false);
			}
			else
			{
				if (invoice.invoiceID == 0)
				{
					MessageBox.Show("Can't load invoice data");
					return;
				}
				this.invoiceID = invoice.invoiceID;
				this.invoiceNote = invoice.invoiceNote;
				this.paymentMethod = null;
				if (Receipt.PaymentMethods != null)
				{
					int num = 0;
					while (num < (int)Receipt.PaymentMethods.Length)
					{
						if (Receipt.PaymentMethods[num].paymentMethodID != invoice.paymentMethodID)
						{
							num++;
						}
						else
						{
							this.paymentMethod = Receipt.PaymentMethods[num];
							break;
						}
					}
				}
				if (this.selectedBill == null)
				{
					this.employeeID = invoice.employeeID;
					this.orderBillID = invoice.orderBillID;
				}
				this.amountDue = invoice.totalPrice;
				this.pointAmount = invoice.point;
				this.tax1 = invoice.tax1;
				this.tax2 = invoice.tax2;
				this.totalDiscount = invoice.totalDiscount;
				this.totalReceive = invoice.totalReceive;
				this.totalDue = this.amountDue + this.tax1 + this.tax2 - this.totalDiscount;
				this.useDiscount.Clear();
				if (invoice.discounts != null)
				{
					for (int i = 0; i < (int)invoice.discounts.Length; i++)
					{
						int num1 = 0;
						while (num1 < (int)Receipt.Discounts.Length)
						{
							if (Receipt.Discounts[num1].promotionID != invoice.discounts[i].promotionID)
							{
								num1++;
							}
							else
							{
								this.useDiscount.Add(Receipt.Discounts[num1]);
								break;
							}
						}
					}
				}
				this.paymentMethodList.Clear();
				this.payValueList.Clear();
				if (invoice.payments != null)
				{
					for (int j = 0; j < (int)invoice.payments.Length; j++)
					{
						int num2 = 0;
						while (num2 < (int)Receipt.PaymentMethods.Length)
						{
							if (Receipt.PaymentMethods[num2].paymentMethodID != invoice.payments[j].paymentMethodID)
							{
								num2++;
							}
							else
							{
								this.paymentMethodList.Add(Receipt.PaymentMethods[num2]);
								this.payValueList.Add(invoice.payments[j].receive);
								break;
							}
						}
					}
					return;
				}
			}
		}

		public void PrintInvoice()
		{
			Invoice invoice = this.CreateInvoice();
			(new smartRestaurant.OrderService.OrderService()).ReprintBill(invoice.orderBillID);
		}

		public static void PrintReceiptAll(OrderInformation orderInfo)
		{
			smartRestaurant.OrderService.OrderService orderService = new smartRestaurant.OrderService.OrderService();
			for (int i = 0; i < (int)orderInfo.Bills.Length; i++)
			{
				if (orderInfo.Bills[i].Items != null && (int)orderInfo.Bills[i].Items.Length > 0 && orderInfo.Bills[i].CloseBillDate == AppParameter.MinDateTime)
				{
					orderService.PrintReceipt(orderInfo.Bills[i].OrderBillID);
				}
			}
		}

		public static Discount SearchDiscountByID(int id)
		{
			for (int i = 0; i < (int)Receipt.discounts.Length; i++)
			{
				if (Receipt.discounts[i].promotionID == id)
				{
					return Receipt.discounts[i];
				}
			}
			return null;
		}

		public static smartRestaurant.PaymentService.PaymentMethod SearchPaymentMethodByID(int id)
		{
			for (int i = 0; i < (int)Receipt.paymentMethods.Length; i++)
			{
				if (Receipt.paymentMethods[i].paymentMethodID == id)
				{
					return Receipt.paymentMethods[i];
				}
			}
			return null;
		}

		public bool SendInvoice(bool closed, bool print)
		{
			bool flag;
			smartRestaurant.CheckBillService.CheckBillService checkBillService = new smartRestaurant.CheckBillService.CheckBillService();
			Invoice invoice = this.CreateInvoice();
			if (!closed)
			{
				invoice.totalReceive = 0;
			}
			string str = checkBillService.SendInvoice(invoice);
			try
			{
				this.invoiceID = int.Parse(str);
				if (!closed && this.invoiceID == -1)
				{
					return false;
				}
				if (print)
				{
					smartRestaurant.OrderService.OrderService orderService = new smartRestaurant.OrderService.OrderService();
					if (!closed)
					{
						orderService.PrintReceipt(invoice.orderBillID);
					}
					else
					{
						orderService.PrintBill(invoice.orderBillID);
					}
				}
				if (this.invoiceID == -1)
				{
					return false;
				}
				return true;
			}
			catch (Exception exception)
			{
				MessageBox.Show(str);
				flag = true;
			}
			return flag;
		}

		public void SetPaymentMethod(smartRestaurant.PaymentService.PaymentMethod method, double val)
		{
			if (method != null)
			{
				int num = this.paymentMethodList.IndexOf(method);
				if (val != 0)
				{
					if (num < 0)
					{
						this.paymentMethodList.Add(method);
						this.payValueList.Add(val);
					}
					else
					{
						this.payValueList[num] = val;
					}
				}
				else if (num >= 0)
				{
					this.paymentMethodList.RemoveAt(num);
					this.payValueList.RemoveAt(num);
				}
			}
			else
			{
				this.paymentMethodList.Clear();
				this.payValueList.Clear();
			}
			this.payValue = 0;
			for (int i = 0; i < this.payValueList.Count; i++)
			{
				this.payValue += (double)this.payValueList[i];
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
	}
}