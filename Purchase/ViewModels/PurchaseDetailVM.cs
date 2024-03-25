using Microsoft.CodeAnalysis;
using Purchase.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Purchase.ViewModels
{
    [NotMapped]
    public class PurchaseDetailVM()
    {
        public PurchaseDetailVM(int productId, decimal quantity, int measurementUnitId, decimal unitPrice) : this()
        {
            ProductId = productId;
            Quantity = quantity;
            MeasurementUnitId = measurementUnitId;
            UnitPrice = unitPrice;
        }

        public PurchaseDetailVM(int productId, decimal quantity, int measurementUnitId, decimal unitPrice, decimal totalPrice) : this(productId, quantity, measurementUnitId, unitPrice)
        {
            TotalPrice = totalPrice;
        }

        public PurchaseDetailVM(int productId, decimal quantity, int measurementUnitId, decimal unitPrice, decimal totalPrice, int purchaseHeaderId) 
            : this(productId, quantity, measurementUnitId, unitPrice, totalPrice)
        {
            PurchaseHeaderId = purchaseHeaderId;
        }

        public PurchaseDetailVM(int id, int productId, decimal quantity, int measurementUnitId, decimal unitPrice, decimal totalPrice, int purchaseHeaderId) : this(productId, quantity, measurementUnitId, unitPrice, totalPrice, purchaseHeaderId)
        {
            Id = id;
        }

        public PurchaseDetailVM(int productId, decimal quantity, int measurementUnitId, decimal unitPrice, decimal totalPrice, int purchaseHeaderId, 
            string productName, string measurementUnitName) : this(productId, quantity, measurementUnitId, unitPrice, totalPrice, purchaseHeaderId)
        {
            ProductName = productName;
            MeasurementUnitName = measurementUnitName;
        }

        public PurchaseDetailVM(int id, int productId, decimal quantity, int measurementUnitId, decimal unitPrice, decimal totalPrice, int purchaseHeaderId, string productName, string measurementUnitName) : this(productId, quantity, measurementUnitId, unitPrice, totalPrice, purchaseHeaderId, productName, measurementUnitName)
        {
            Id = id;
        }

        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [AllowNull, Display(Name = "Product")]
        public string ProductName { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public int MeasurementUnitId { get; set; }

        [AllowNull, Display(Name = "Measurement Unit")]
        public string MeasurementUnitName { get; set; }

        [Required, Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [AllowNull, Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        [Required, Display(Name = "Purchase Header")]
        public int PurchaseHeaderId { get; set; }

        [AllowNull]
        public MeasurementUnitVM? MeasurementUnit { get; set; }
        [AllowNull]
        public ProductVM? Product { get; set; }
        [AllowNull]
        public PurchaseHeaderVM? PurchaseHeader { get; set; }
    }
}
