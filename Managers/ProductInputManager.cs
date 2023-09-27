using static System.Console;

namespace ProductManager.Managers;

public class ProductInputManager
{
    public static (
        string Name,
        string SKU,
        string Description,
        string Image,
        int Price
    ) CollectProductData()
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

        return (productName, productSKU, productDescription, productImage, productPrice);
    }
}
