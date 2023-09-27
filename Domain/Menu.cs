using static System.Console;

namespace ProductManager.Domain;

public class MenuManager
{
    public static void InitializeMenu()
    {
        WriteLine("1. Ny produkt");
        WriteLine("2. Sök produkt");
        WriteLine("3. Ny kategori");
        WriteLine("4. Lägg till produkt till kategori");
        WriteLine("5. Lista kategorier");
    }
}
