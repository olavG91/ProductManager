using ProductManager.Domain;
using static System.Console;

namespace ProductManager.Managers;

public class ProductInputManager
{
    public static Product CollectProductData()
    {
        string productName;
        string productSKU;
        string productDescription;
        string productImage;
        int productPrice;

        WriteLine("Namn:");
        productName = ReadLine() ?? "";

        WriteLine("SKU:");
        productSKU = ReadLine() ?? "";

        WriteLine("Beskrivning:");
        productDescription = ReadLine() ?? "";

        WriteLine("Bild (URL):");
        productImage = ReadLine() ?? "";

        int price;
        WriteLine("Pris:");
        if (int.TryParse(ReadLine(), out price))
        {
            productPrice = price;
        }
        else
        {
            productPrice = 0;
        }

        return new Product
        {
            Name = productName,
            SKU = productSKU,
            Description = productDescription,
            Image = productImage,
            Price = productPrice
        };
    }

    public static Product UpdateProductData(Product product)
    {
        string productName = product.Name;
        string productSKU = product.SKU;
        string productDescription = product.Description;
        string productImage = product.Image;
        int productPrice = product.Price;
        string input;

        WriteLine("Namn (" + productName + "):");
        input = ReadLine() ?? "";
        productName = string.IsNullOrEmpty(input) ? product.Name : input;

        WriteLine("Beskrivning (" + productDescription + "):");
        input = ReadLine() ?? "";
        productDescription = string.IsNullOrEmpty(input) ? product.Description : input;

        WriteLine("Bild (" + productImage + ") (URL):");
        input = ReadLine() ?? "";
        productImage = string.IsNullOrEmpty(input) ? product.Image : input;

        int price;
        WriteLine("Pris (" + productPrice + "):");
        if (int.TryParse(ReadLine(), out price))
        {
            productPrice = price;
        }
        else
        {
            productPrice = product.Price;
        }

        return new Product
        {
            Name = productName,
            SKU = productSKU,
            Description = productDescription,
            Image = productImage,
            Price = productPrice
        };
    }
}
