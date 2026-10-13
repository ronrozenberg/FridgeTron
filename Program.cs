using System.ComponentModel;
using System.Drawing;

namespace FridgeTron
{
    public class Program
    {
        public static void PrintItems(List<Item> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null) Console.WriteLine(items[i].ToString());
            }
        }

        public static void PrintList(List<string> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null) Console.Write(items[i] + " ");
            }
        }

        public static DateTime GetCurrDate()
        {
            DateTime now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
        }

        public static void Main(string[] args)
        {
            Category dairy = new Category("Dairy", 2, 6);
            Category fruits = new Category("Fruits", 3, 6);
            // level 1
            Console.WriteLine("=======level 1=======");
            Item banana = new Item("Banana", fruits, new DateTime(2012, 12, 12, 12, 12, 0), 12);
            Item strawberry = new Item("Strawberry", fruits, new DateTime(2027, 12, 12, 12, 12, 0), 5);
            Item butter = new Item("Butter", dairy, new DateTime(2026, 12, 12, 12, 12, 0), 11);
            Fridge fridge = new Fridge(3, 1, 100);
            fridge.AddNewItem(banana);
            fridge.AddNewItem(strawberry);
            fridge.AddNewItem(butter);
            Console.WriteLine("All:");
            PrintItems(fridge.ListAll());
            Console.WriteLine("Bad:");
            List<Item> badItems = fridge.Find(item => item.GetexDate() < DateTime.Now, fridge.ListAll());
            PrintItems(badItems);
            Console.WriteLine("Adding another banana will print false:");
            Console.WriteLine(fridge.AddNewItem(banana));
            Console.WriteLine("Removing Bad...");
            fridge.RemoveBy(item => item.GetexDate() < DateTime.Now);
            Console.WriteLine("All:");
            PrintItems(fridge.ListAll());

            Console.WriteLine("Extending fridge from 3->4...");
            fridge.SetmaxItems(4);
            PrintItems(fridge.ListAll());
            Console.WriteLine("Adding another banana will print true:");
            Console.WriteLine(fridge.AddNewItem(banana));
            Item pear = new Item("Pear", fruits, new DateTime(2012, 12, 12, 12, 12, 0), 12);
            Console.WriteLine("Adding a pear will print true:");
            Console.WriteLine(fridge.AddNewItem(pear));
            Console.WriteLine("Adding another banana will print false:");
            Console.WriteLine(fridge.AddNewItem(banana));
            PrintItems(fridge.ListAll());

            //level 2 part 1
            Console.WriteLine("=======level 2=======");
            Item a = new Item("Banana", fruits, new DateTime(2026, 11, 12, 12, 12, 0), 12);
            Item b = new Item("Strawberry", fruits, new DateTime(2027, 12, 12, 12, 12, 0), 5);
            Item c = new Item("Banana", fruits,  new DateTime(2026, 12, 12, 12, 12, 0), 12);

            Fridge fridge2 = new Fridge(3, 4, 100);
            fridge2.AddNewItem(a);
            fridge2.AddNewItem(b);
            fridge2.AddNewItem(c);
            PrintItems(fridge2.ListAll());
            Console.WriteLine("Sorted by ex date:");
            PrintItems(fridge2.SortByExDate());

            Console.WriteLine("Removing by 'Banana' will remove the one whose date's is closer");
            fridge2.RemoveItem("Banana");
            PrintItems(fridge2.ListAll());

            //level 2 part 2
            Category icy = new Category("Icy", 0, 0);
            List<Compartment> compartments = new List<Compartment>();
            compartments.Add(new Compartment("Freezer", 20, 0, new List<int> { 3 }, 0, 3));
            Fridge freezer = new Fridge(3, -18, 100, compartments);
            Item ice = new Item("Ice", icy, new DateTime(2027, 12, 12, 12, 12, 0), 5);
            Console.WriteLine("Adding ice to freezer will print true:");
            Console.WriteLine(freezer.AddItemToCompartment(ice,1));
            Console.WriteLine("Adding ice to fridge will print false:");
            Console.WriteLine(fridge2.AddNewItem(ice));
            Console.WriteLine("Adding too much Size will print false:");
            Item heavyItem = new Item("Watermelon", fruits, new DateTime(2027, 12, 12, 12, 12, 0), 1000);
            Console.WriteLine(fridge2.AddNewItem(heavyItem));
            Console.WriteLine("Freezer:");
            PrintItems(freezer.ListAll());
            Console.WriteLine("Fridge:");
            PrintItems(fridge2.ListAll());
            Console.WriteLine("Size sum of both:");
            Console.WriteLine(freezer.SumSize() + fridge2.SumSize());

            Console.WriteLine("==============");
            Item a1 = new Item("Banana", fruits, new DateTime(2012, 11, 12, 12, 12, 0), 12);
            Item b2 = new Item("Strawberry", fruits, new DateTime(2027, 12, 12, 12, 12, 0), 5);
            Item c3 = new Item("Banana", fruits, new DateTime(2026, 10, 10, 12, 12, 0), 12);

            Fridge fridge3 = new Fridge(3, 4, 100);
            fridge3.AddNewItem(a1);
            fridge3.AddNewItem(b2);
            fridge3.AddNewItem(c3);
            PrintItems(fridge3.ListAll());
            Console.WriteLine("Opening Strawberries:");
            b2.Open();
            PrintItems(fridge3.ListAll());
            Console.WriteLine("Removed This Month:");
            fridge3.RemoveBy(item => item.GetexDate() < DateTime.Now);
            PrintItems(GlobalIDs.GetThrownThisMonth());
            Console.WriteLine("All categories in program:");
            PrintList(GlobalIDs.Getcategories());
        }
    }
}