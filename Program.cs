using static System.Console;
using ProductManager.Domain;
using ProductManager.Data;
using ProductManager.Managers;

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
        var (productName, productSKU, productDescription, productImage, productPrice) =
            ProductInputManager.CollectProductData();

        Clear();

        WriteLine("Namn: " + productName);
        WriteLine("SKU: " + productSKU);
        WriteLine("Beskrivning: " + productDescription);
        WriteLine("Bild (URL): " + productImage);
        WriteLine("Pris: " + productPrice);

        WriteLine("Är detta korekt? (J)a (N)ej");

        ConsoleKeyInfo keyInfo = ReadKey(true);
        if (keyInfo.Key == ConsoleKey.J)
        {
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
        else
        {
            Clear();
            AddProduct();
        }
    }
}
