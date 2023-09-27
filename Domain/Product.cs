using System.ComponentModel;

namespace ProductManager.Domain;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string SKU { get; set; }
    public required string Description { get; set; }
    public required string Image { get; set; }
    public required int Price { get; set; }

    public int? CategoryId { get; set; }
}
