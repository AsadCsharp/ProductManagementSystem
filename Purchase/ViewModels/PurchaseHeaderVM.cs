using Purchase.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Purchase.ViewModels
{
    [NotMapped]
    public class PurchaseHeaderVM()
    {
        public PurchaseHeaderVM(string customerName, string customerPhoneNumber, string customerEmailAddress, string invoiceNumber, DateTime purchaseDate, decimal totalAmount) : this() 
        {
            CustomerName = customerName;
            CustomerPhoneNumber = customerPhoneNumber;
            CustomerEmailAddress = customerEmailAddress;
            InvoiceNumber = invoiceNumber;
            PurchaseDate = purchaseDate;
            TotalAmount = totalAmount;
        }

        public PurchaseHeaderVM(int id, string customerName, string customerPhoneNumber, string customerEmailAddress, string invoiceNumber, DateTime purchaseDate, decimal totalAmount) : this(customerName, customerPhoneNumber, customerEmailAddress, invoiceNumber, purchaseDate, totalAmount)
        {
            Id = id;
        }

        public int Id { get; set; }

        [AllowNull, Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = string.Empty;

        [AllowNull, DataType(DataType.PhoneNumber), Display(Name = "Custome Phone Number")]
        public string CustomerPhoneNumber { get; set; }

        [AllowNull, DataType(DataType.EmailAddress), Display(Name = "Custome Email Address")]
        public string CustomerEmailAddress { get; set; }

        [AllowNull, Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; }

        [AllowNull, DataType(DataType.Date), Column(TypeName = "DATE"), Display(Name = "Purchase Date")]
        public DateTime PurchaseDate { get; set; }

        [AllowNull, Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [AllowNull]
        public ICollection<PurchaseDetailVM> PurchaseDetails { get; set; } = new List<PurchaseDetailVM>();
    }
}
