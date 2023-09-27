using static System.Console;
using ProductManager.Domain;
using ProductManager.Data;
using ProductManager.Managers;
using Microsoft.EntityFrameworkCore;

namespace ProductManager;

class Program
{
    static void Main()
    {
        Dictionary<ConsoleKey, Action> actions = new Dictionary<ConsoleKey, Action>
        {
            { ConsoleKey.D1, AddProduct },
            { ConsoleKey.NumPad1, AddProduct },
            { ConsoleKey.D2, SearchProduct },
            { ConsoleKey.NumPad2, SearchProduct },
            { ConsoleKey.D3, AddCategory },
            { ConsoleKey.NumPad3, AddCategory },
            { ConsoleKey.D4, AddProductToCategory },
            { ConsoleKey.NumPad4, AddProductToCategory },
            { ConsoleKey.D5, ListCategories },
            { ConsoleKey.NumPad5, ListCategories },
            { ConsoleKey.D6, () => Environment.Exit(0) },
            { ConsoleKey.NumPad6, () => Environment.Exit(0) }
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
        Clear();

        Product newProduct = ProductInputManager.CollectProductData();

        Clear();

        ShowProductDetails(newProduct);

        WriteLine("Är detta korekt? (J)a (N)ej");

        ConsoleKeyInfo keyInfo = ReadKey(true);
        if (keyInfo.Key == ConsoleKey.J)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Products.Add(newProduct);
                context.SaveChanges();
            }

            Clear();

            ShowConfirmation("Produkt sparad", false);
        }
        else if (keyInfo.Key == ConsoleKey.N)
        {
            Clear();
            AddProduct();
        }
        else
        {
            WaitUntil(ConsoleKey.Escape);
            Clear();
        }
    }

    public static void SearchProduct()
    {
        Clear();
        WriteLine("SKU:");
        string SKU = ReadLine() ?? "";

        using (var context = new ApplicationDbContext())
        {
            var products = context.Products.Where(a => a.SKU.Contains(SKU)).ToList();

            if (products.Count > 0)
            {
                foreach (var product in products)
                {
                    ShowProduct(product.SKU);
                }
            }
            else
            {
                Clear();
                ShowConfirmation("Ingen produkt hittad", false);
            }
        }
    }

    public static void ShowProduct(string SKU)
    {
        using (var context = new ApplicationDbContext())
        {
            var product = context.Products.FirstOrDefault(a => a.SKU == SKU);

            if (product != null)
            {
                Clear();
                ShowProductDetails(product);

                WriteLine("(R)adera (U)ppdatera");
                ConsoleKeyInfo keyInfo = ReadKey(true);
                if (keyInfo.Key == ConsoleKey.R)
                {
                    DeleteProduct(product);
                }
                else if (keyInfo.Key == ConsoleKey.U)
                {
                    UpdateProduct(product);
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Clear();
                }
            }
            else
            {
                Clear();
                ShowConfirmation("Ingen produkt hittad", false);
            }
        }
    }

    public static void UpdateProduct(Product product)
    {
        Clear();

        Product updatedProduct = ProductInputManager.UpdateProductData(product);

        ShowProductDetails(updatedProduct);

        WriteLine("Är detta korrekt (J)a (N)ej");

        ConsoleKeyInfo keyInfo = ReadKey(true);

        if (keyInfo.Key == ConsoleKey.J)
        {
            using (var context = new ApplicationDbContext())
            {
                var productToUpdate = context.Products.FirstOrDefault(a => a.SKU == product.SKU);
                if (productToUpdate != null)
                {
                    productToUpdate.Name = updatedProduct.Name;
                    productToUpdate.Description = updatedProduct.Description;
                    productToUpdate.Image = updatedProduct.Image;
                    productToUpdate.Price = updatedProduct.Price;

                    context.SaveChanges();
                    Clear();
                    ShowConfirmation("Produkt uppdaterad", false);
                }
                else
                {
                    ShowConfirmation("Ingen produkt att uppdatera.", false);
                }
            }
        }
        else if (keyInfo.Key == ConsoleKey.N)
        {
            Clear();
            UpdateProduct(product);
        }
        else
        {
            WaitUntil(ConsoleKey.Escape);
            Clear();
        }
    }

    public static void DeleteProduct(Product product)
    {
        Clear();
        ShowProductDetails(product);
        WriteLine("Radera produkt? (J)a (N)ej");
        ConsoleKeyInfo keyInfo = ReadKey(true);
        if (keyInfo.Key == ConsoleKey.J)
        {
            using (var context = new ApplicationDbContext())
            {
                var productToDelete = context.Products.FirstOrDefault(a => a.SKU == product.SKU);
                if (productToDelete != null)
                {
                    context.Products.Remove(productToDelete);
                    context.SaveChanges();
                    Clear();
                    ShowConfirmation("Produkt borttagen.", false);
                }
                else
                {
                    ShowConfirmation("Ingen produkt att ta bort.", false);
                }
            }
        }
        else if (keyInfo.Key == ConsoleKey.N)
        {
            ShowProduct(product.SKU);
            Clear();
        }
    }

    public static void AddCategory()
    {
        Clear();
        WriteLine("Namn:");
        string category = ReadLine() ?? "";
        WriteLine("Namn: " + category);
        WriteLine("Är detta korrekt? (J)a (N)ej");
        ConsoleKeyInfo keyInfo = ReadKey(true);
        if (keyInfo.Key == ConsoleKey.J)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Categories.Add(new Category { Name = category });
                context.SaveChanges();
                ShowConfirmation("Kategori sparad", false);
            }
        }
        else if (keyInfo.Key == ConsoleKey.N)
        {
            Clear();
            AddCategory();
        }
        else
        {
            WaitUntil(ConsoleKey.Escape);
            Clear();
        }
    }

    public static void AddProductToCategory()
    {
        Clear();
        WriteLine("SKU:");
        string SKU = ReadLine() ?? "";
        using (var context = new ApplicationDbContext())
        {
            try
            {
                var product = context.Products.FirstOrDefault(a => a.SKU == SKU);

                if (product == null)
                {
                    Clear();
                    ShowConfirmation("Produkt hittades inte", false);
                    return;
                }

                WriteLine("Namn: " + product.Name);
                WriteLine("SKU: " + product.SKU);
                WriteLine("Ange kategori:");
                string setCategory = ReadLine() ?? "";
                var category = context.Categories.FirstOrDefault(a => a.Name == setCategory);

                if (category == null)
                {
                    ShowConfirmation("Kategori finns inte", false);
                    return;
                }

                if (product.Category != null && product.Category.Id == category.Id)
                {
                    ShowConfirmation("Produkt redan tillagd", false);
                    return;
                }

                product.Category = category;
                category.Products.Add(product);
                context.SaveChanges();
                ShowConfirmation("Produkt tillagd", false);
            }
            catch (Exception ex)
            {
                ShowConfirmation($"Ett fel inträffade: {ex.Message}", false);
            }
        }
    }

    public static void ListCategories()
    {
        Clear();
        using (var context = new ApplicationDbContext())
        {
            var categories = context.Categories.Include(c => c.Products).ToList();
            if (categories.Count > 0)
            {
                foreach (var category in categories)
                {
                    int productCount = category.Products.Count;
                    WriteLine($"{category.Name} ({productCount})");

                    foreach (var product in category.Products)
                    {
                        WriteLine($"    {product.Name}      {product.Price} SEK");
                    }
                }

                WaitUntil(ConsoleKey.Escape);
                Clear();
            }
            else
            {
                ShowConfirmation("Inga kategorier hittade.", false);
            }
        }
    }

    public static void ShowProductDetails(Product product)
    {
        WriteLine("Namn: " + product.Name);
        WriteLine("SKU: " + product.SKU);
        WriteLine("Beskrivning: " + product.Description);
        WriteLine("Bild (URL): " + product.Image);
        WriteLine("Pris: " + product.Price);
    }

    public static void ShowConfirmation(string message, bool enableInput)
    {
        WriteLine(message);
        if (enableInput)
        {
            WriteLine("Tryck på en valfri knapp för att fortsätta.");
            ReadKey();
            Clear();
        }
        else
        {
            Thread.Sleep(2000);
            Clear();
        }
    }

    public static void WaitUntil(ConsoleKey key)
    {
        while (ReadKey(true).Key != key)
            ;
    }
}
