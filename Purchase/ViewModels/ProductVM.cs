using Purchase.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Purchase.ViewModels
{
    [NotMapped]
    public class ProductVM()
    {
        public ProductVM(string name) : this()
        {
            Name = name;
        }

        public ProductVM(string name, string imageUrl) : this(name)
        {
            ImageUrl = imageUrl;
        }

        public ProductVM(int id, string name, string imageUrl) : this(name, imageUrl)
        {
            Id = id;
        }

        public int Id { get; set; } = 0;

        [Required]
        public string Name { get; set; } = string.Empty;

        [AllowNull, DataType(DataType.ImageUrl), Display(Name = "Image")]
        public string ImageUrl { get; set; }

    }
}
