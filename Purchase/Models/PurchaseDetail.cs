using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Purchase.Models
{
    [Table(nameof(PurchaseDetail))]
    public class PurchaseDetail()
    {
        public PurchaseDetail(int productId, decimal quantity, int measurementUnitId, decimal productUnitPrice, decimal productTotalPrice, int purchaseHeaderId) : this()
        {
            ProductId = productId;
            Quantity = quantity;
            MeasurementUnitId = measurementUnitId;
            ProductUnitPrice = productUnitPrice;
            ProductTotalPrice = productTotalPrice;
            PurchaseHeaderId = purchaseHeaderId;
        }

        public PurchaseDetail(int id, int productId, decimal quantity, int measurementUnitId, decimal productUnitPrice, decimal productTotalPrice, int purchaseHeaderId) : this(productId, quantity, measurementUnitId, productUnitPrice, productTotalPrice, purchaseHeaderId)
        {
            Id = id;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [ForeignKey(nameof(MeasurementUnit))]
        public int MeasurementUnitId { get; set; }

        [Required]
        public decimal ProductUnitPrice { get; set; }

        [AllowNull]
        public decimal ProductTotalPrice { get; set; }

        [ForeignKey(nameof(PurchaseHeader))]
        public int PurchaseHeaderId { get; set; }

        [AllowNull]
        public MeasurementUnit? MeasurementUnit { get; set; }
        [AllowNull]
        public Product? Product { get; set; }
        [AllowNull]
        public PurchaseHeader? PurchaseHeader { get; set; }
    }
}
