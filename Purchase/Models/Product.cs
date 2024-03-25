using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Purchase.Models
{
    [Table(nameof(Product))]
    public class Product()
    {
        public Product(string name) : this()
        {
            Name = name;
        }

        public Product(string name, string imageUrl) : this(name)
        {
            ImageUrl = imageUrl;
        }

        public Product(int id, string name, string imageUrl) : this(name, imageUrl)
        {
            Id = id;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [AllowNull, DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [AllowNull]
        public ICollection<PurchaseDetail> purchaseDetails { get; set; }
    }
}
