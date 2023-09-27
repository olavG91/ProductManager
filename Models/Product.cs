using System.ComponentModel;

namespace ProductManager.Models;

public class Product
{
    public string Name { get; set; }
    public string SKU { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public int Price { get; set; }
}
