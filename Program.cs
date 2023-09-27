using static System.Console;
using ProductManager.Domain;
using ProductManager.Data;

namespace ProductManager;

class Program
{
    static void Main()
    {
        Dictionary<ConsoleKey, Action> actions = new Dictionary<ConsoleKey, Action>
        {
            { ConsoleKey.D1, AddProduct },
            { ConsoleKey.NumPad1, AddProduct },
            { ConsoleKey.D5, () => Environment.Exit(0) },
            { ConsoleKey.NumPad5, () => Environment.Exit(0) }
        };

        while (true)
        {
            MenuManager.InitializeMenu();

            ConsoleKey key = ReadKey(true).Key;

            if (actions.ContainsKey(key))
            {
                actions[key].Invoke();
            }
            else
            {
                WriteLine("Ogiltigt val, försök igen");
            }
        }
    }

    public static void AddProduct()
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

        Product newProduct = new Product
        {
            Name = productName,
            SKU = productSKU,
            Description = productDescription,
            Image = productImage,
            Price = productPrice
        };

        using (var context = new ApplicationDbContext())
        {
            context.Products.Add(newProduct);
            context.SaveChanges();
        }

        Clear();
    }
}
