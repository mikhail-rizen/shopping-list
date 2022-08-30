using System.ComponentModel.DataAnnotations;

namespace ShoppingList.Items.Api.Models
{
    public class CreateItemModel
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;
    }
}
