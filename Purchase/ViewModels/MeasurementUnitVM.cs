    using System.ComponentModel.DataAnnotations.Schema;

namespace Purchase.ViewModels
{
    [NotMapped]
    public class MeasurementUnitVM
    {
        public MeasurementUnitVM() { }

        public MeasurementUnitVM(string name): this()
        {
            Name = name;
        }

        public MeasurementUnitVM(int id, string name) : this(name)
        {
            Id = id;
        }

        public int Id { get; set; } = 0;

        public string Name { get; set; }
    }
}
