using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Purchase.Models
{
    [Table(nameof(MeasurementUnit))]
    public class MeasurementUnit
    {
        public MeasurementUnit() { }

        public MeasurementUnit(string name): this()
        {
            Name = name;
        }

        public MeasurementUnit(int id, string name):this(name)
        {
            Id = id;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        [AllowNull]
        public ICollection<PurchaseDetail>? PurchaseDetails { get; set; }
    }
}
