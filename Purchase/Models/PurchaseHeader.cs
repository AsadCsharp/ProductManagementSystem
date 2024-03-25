using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Purchase.Models
{
    [Table(nameof(PurchaseHeader))]
    public class PurchaseHeader()
    {
        public PurchaseHeader(string invoiceNumber, DateTime purchaseDate, decimal totalBill, string customerName = "", string customerPhoneNumber = "", string customerEmailAddress = "") : this()
        {
            InvoiceNumber = invoiceNumber;
            PurchaseDate = purchaseDate;
            TotalBill = totalBill;
            CustomerName = customerName;
            CustomerPhoneNumber = customerPhoneNumber;
            CustomerEmailAddress = customerEmailAddress;
        }

        public PurchaseHeader(int id, string invoiceNumber, DateTime purchaseDate, decimal totalBill, string customerName = "", string customerPhoneNumber = "", string customerEmailAddress = "") : this(invoiceNumber, purchaseDate, totalBill, customerName, customerPhoneNumber, customerEmailAddress)
        {
            Id = id;
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [AllowNull]
        public string? CustomerName { get; set; }

        [AllowNull]
        public string? CustomerPhoneNumber { get; set; }

        [AllowNull]
        public string? CustomerEmailAddress { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }

        [Required, DataType(DataType.Date), Column(TypeName = "DATE")]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public decimal TotalBill { get; set; }

        public ICollection<PurchaseDetail> purchaseDetails { get; set; } = new List<PurchaseDetail>();
    }
}
