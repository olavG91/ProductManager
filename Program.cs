using static System.Console;
using ProductManager.Domain;

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
        Product newProduct = new Product();
        WriteLine("Namn:");
        newProduct.Name = ReadLine();

        WriteLine("SKU:");
        newProduct.SKU = ReadLine();

        WriteLine("Beskrivning:");
        newProduct.Description = ReadLine();

        WriteLine("Bild (URL):");
        newProduct.Image = ReadLine();

        WriteLine("Pris:");
        newProduct.Price = ReadLine();

        using (var context = new ApplicationDbContext())
        {
            context.Products.Add(newProduct);
            context.SaveChanges();
        }
    }
}
