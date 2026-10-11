using System.ComponentModel;
using System.Drawing;

namespace FridgeTron
{
    public class Program
    {
        public class Fridge
        {
            private int maxItems;
            private double currentTemp;
            private List<Item> itemList = new List<Item>();
            private List<Item> thrownThisMonth = new List<Item>();
            private double maxSize;
            private List<Compartment> compartments = new List<Compartment>();

            public Fridge(int maxItems, double currentTemp, double maxSize)
            {
                if (maxItems <= 0) throw new ArgumentException("maxItems must be greater than 0");
                if (maxSize <= 0) throw new ArgumentException("maxSize must be greater than 0");
                this.maxItems = maxItems;
                this.currentTemp = currentTemp;
                this.itemList = new List<Item>();
                this.maxSize = maxSize;
            }

            public Fridge(int maxItems, double currentTemp, double maxSize, List<Compartment> compartments)
            {
                if (maxItems <= 0) throw new ArgumentException("maxItems must be greater than 0");
                if (maxSize <= 0) throw new ArgumentException("maxSize must be greater than 0");
                this.maxItems = maxItems;
                this.currentTemp = currentTemp;
                this.itemList = new List<Item>();
                this.maxSize = maxSize;
                if(compartments != null) this.compartments = compartments;
            }

            public bool AddNewItem(Item item)
            {
                if (item == null) return false;
                if (this.CountItems() >= this.maxItems) return false;
                if (this.SumSize() + item.GetSize() > this.maxSize) return false;
                if (this.CheckDupe(item, this.itemList)) return false;
                if (item.Getcategory().GetneededTemp() < this.currentTemp) return false;
                for (int i = 0; i < this.itemList.Count; i++)
                {
                    if (this.itemList[i] == null)
                    {
                        this.itemList[i] = item;
                        return true;
                    }
                }
                if (this.itemList.Count < this.maxItems)
                {
                    this.itemList.Add(item);
                    return true;
                }
                return false;
            }

            public double SumSize()
            {
                double sum = 0;
                for (int i = 0; i < this.itemList.Count; i++)
                {
                    if (this.itemList[i] != null) sum += this.itemList[i].GetSize();
                }
                return sum;
            }

            public bool CheckDupe(Item item, List<Item> itemList)
            {
                if (item == null) return false;
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (itemList[i] != null && itemList[i].IsIdentical(item)) return true;
                }
                return false;
            }

            public int CountItems()
            {
                int count = 0;
                for (int i = 0; i < this.itemList.Count; i++)
                {
                    if (this.itemList[i] != null) count++;
                }
                return count;
            }

            public bool RemoveItem(int index)
            {
                if (index < 0 || index >= this.itemList.Count) return false;
                if (this.itemList[index] == null) return false;
                itemList[index] = null;
                return true;
            }

            public bool RemoveItem(Item item)
            {
                if (item == null) return false;
                for (int i = 0; i < this.itemList.Count; i++)
                {
                    if (this.itemList[i] != null && this.itemList[i] == item)
                    {
                        itemList[i] = null;
                        return true;
                    }
                }
                return false;
            }

            public bool RemoveItem(string item)
            {
                if (item == null) return false;
                List<Item> foundItems = this.Find(i => i.Getname() == item);
                int smallestIndex = 0;
                if (foundItems.Count == 0) return false;
                if (foundItems.Count == 1)
                {
                    RemoveItem(foundItems[0]);
                    return true;
                }
                if (foundItems.Count > 1)
                {
                    DateTime closestDate = foundItems[0].GetexDate();
                    for (int i = 0; i < foundItems.Count; i++)
                    {
                        if (foundItems[i] != null && closestDate > foundItems[i].GetexDate())
                        {
                            closestDate = foundItems[i].GetexDate();
                            smallestIndex = i;
                        }
                    }
                    RemoveItem(foundItems[smallestIndex]);
                    return true;
                }
                return false;
            }

            public List<Item> ListAll()
            {
                List<Item> result = new List<Item>();

                for (int i = 0; i < this.itemList.Count; i++)
                {
                    if (this.itemList[i] != null)
                    {
                        result.Add(this.itemList[i]);
                    }
                }

                return result;
            }

            public List<Item> Find(Func<Item, bool> condition)
            {
                List<Item> foundItems = new List<Item>();
                for (int i = 0; i < this.itemList.Count; i++)
                {
                    if (this.itemList[i] != null && condition(this.itemList[i]))
                    {
                        foundItems.Add(this.itemList[i]);
                    }
                }
                return foundItems;
            }

            public List<Item> SortByExDate()
            {
                List<Item> sortedItems = new List<Item>(this.ListAll());
                for(int i = 0; i < sortedItems.Count - 1; i++)
                {
                    for (int j = 0; j < sortedItems.Count - i - 1; j++)
                    {
                        if (sortedItems[j].GetexDate() > sortedItems[j + 1].GetexDate())
                        {
                            Item temp = sortedItems[j];
                            sortedItems[j] = sortedItems[j + 1];
                            sortedItems[j + 1] = temp;
                        }
                    }
                }
                return sortedItems;
            }

            public void RemoveBy(Func<Item, bool> condition)
            {
                for (int i = 0; i < this.itemList.Count; i++)
                {
                    if (this.itemList[i] != null && condition(this.itemList[i]))
                    {
                        if (!CheckDupe(this.itemList[i], this.thrownThisMonth))
                        {
                            this.thrownThisMonth.Add(this.itemList[i]);
                            thrownThisMonth[thrownThisMonth.Count - 1].SetdateOfThrowing(DateTime.Now);
                        }
                        this.itemList[i] = null;
                    }
                }
            }

            public void SetmaxItems(int num)
            {
                if (num <= 0) throw new ArgumentException("maxItems must be greater than 0");
                List<Item> newitemList = new List<Item>();

                if (num < this.maxItems)
                {
                    for (int i = 0; i < num; i++)
                    {
                        newitemList.Add(this.itemList[i]);
                    }
                    this.itemList = newitemList;
                }
                this.maxItems = num;
            }

            public void AddCompartment(Compartment compartment)
            {
                if (compartment == null) throw new ArgumentException("compartment must be set");
                this.compartments.Add(compartment);
            }

            public bool AddItemToCompartment(Item item, int id)
            {
                if (item == null) throw new ArgumentException("item must be set");
                if (id == null) throw new ArgumentException("compartment must be set");
                for(int i = 0; i < this.compartments.Count; i++)
                {
                    if (this.compartments[i] != null && this.compartments[i].GetID() == id)
                    {
                        if(compartments[i].AddItem(item)) return true;
                    }
                }
                return false;
            }
            public void Setcompartments(List<Compartment> compartments)
            {
                this.compartments = compartments;
            }

            public void SetmaxSize(double num)
            {
                if (num <= 0) throw new ArgumentException("maxSize must be greater than 0");
                this.maxSize = num;
            }

            public void SetcurrentTemp(double num)
            {
                this.currentTemp = num;
            }

            public List<Compartment> Getcompartments()
            {
                return this.compartments;
            }

            public int GetmaxItems()
            {
                return this.maxItems;
            }

            public double GetcurrentTemp()
            {
                return this.currentTemp;
            }

            public double GetmaxSize()
            {
                return this.maxSize;
            }

            public List<Item> GetThrownThisMonth()
            {
                List<Item> list = new List<Item>();
                for (int i = this.thrownThisMonth.Count - 1; i >= 0; i--)
                {
                    if (this.thrownThisMonth[i] == null || (DateTime.Now - this.thrownThisMonth[i].GetdateOfThrowing().Date).Days >= 30)
                        this.thrownThisMonth.RemoveAt(i);
                }
                for (int i = 0; i < this.thrownThisMonth.Count; i++)
                {
                    if (this.thrownThisMonth[i] != null)
                        list.Add(this.thrownThisMonth[i]);
                }
                return list;
            }
        }

        public class Compartment
        {
            private string name;
            private int type;
            private double size;
            private int id;
            private double currentTemp;
            private List<int> allowedCategories;
            private List<Item> items = new List<Item>();

            public Compartment(string name, double size, int type, List<int> allowedCategories, double currentTemp)
            {
                this.name = name;
                this.size = size;
                this.id = GlobalIDs.GetCompartmentID();
                this.type = type;
                this.allowedCategories = allowedCategories;
                this.currentTemp = currentTemp;
            }

            public double SumSize()
            {
                double sum = 0;
                for (int i = 0; i < this.items.Count; i++)
                {
                    if (this.items[i] != null) sum += this.items[i].GetSize();
                }
                return sum;
            }

            public bool AddItem(Item item)
            {
                if (item == null) throw new ArgumentException("item must be set");
                if (!this.allowedCategories.Contains(item.Getcategory().Gettype())) return false;
                if (item.Getcategory().GetneededTemp() > this.currentTemp) return false;
                if (this.SumSize() + item.GetSize() > this.size) throw new ArgumentException("compartment size exceeded");
                this.items.Add(item);
                return true;
            }

            public List<Item> GetItems()
            {
                return this.items;
            }

            public int GetID()
            {
                return this.id;
            }
        }

        public class Category
        {
            private string name;
            private int type;
            private int exdays;
            private int id;
            private double neededTemp;

            public Category(string name, int exdays, double neededTemp)
            {
                this.name = name;
                if (GlobalIDs.GetCategoryType(name) == -1) GlobalIDs.Addcategory(name);
                this.type = GlobalIDs.GetCategoryType(name);
                this.id = GlobalIDs.GetCategoryID();
                this.exdays = exdays;
                this.neededTemp = neededTemp;
            }
            public string Getname()
            {
                return this.name;
            }
            public int Gettype()
            {
                return this.type;
            }
            public int Getexdays()
            {
                return this.exdays;
            }

            public double GetneededTemp()
            {
                return this.neededTemp;
            }
        }

        public class GlobalIDs
        {
            private static int ItemID = 0;
            private static int CompartmentID = 0;
            private static int CategoryID = 0;
            private static List<string> categories = ["Dairy", "Fruits", "Drinks"];
            public static int GetItemID()
            {
                ItemID++;
                return ItemID;
            }
            public static int GetCompartmentID()
            {
                CompartmentID++;
                return CompartmentID;
            }
            public static int GetCategoryID()
            {
                CategoryID++;
                return CategoryID;
            }
            public static List<string> Getcategories()
            {
                return categories;
            }
            public static int GetCategoryType(string category)
            {
                for (int i = 0; i < categories.Count; i++)
                {
                    if (categories[i].Equals(category))
                        return i;
                }
                return -1;
            }
            public static void Addcategory(string category)
            {
                categories.Add(category);
            }

        }
        public class Item
        {
            private string name;
            private Category category;
            private DateTime exDate;
            private double Size;
            private int id;
            private DateTime dateOfThrowing;

            public Item(string name, string category,int exdays, double temp, DateTime exDate, double Size)
            {
                if (Size <= 0) throw new ArgumentException("Size must be greater than 0");
                if (exDate == default) throw new ArgumentException("exDate must be set");
                this.name = name;
                this.category = new Category(category, exdays, temp);
                this.exDate = exDate;
                this.Size = Size;
                this.id = GlobalIDs.GetItemID();
            }

            public Item(string name, Category category, DateTime exDate, double Size)
            {
                if (Size <= 0) throw new ArgumentException("Size must be greater than 0");
                if (exDate == default) throw new ArgumentException("exDate must be set");
                this.name = name;
                this.category = category;
                this.exDate = exDate;
                this.Size = Size;
                this.id = GlobalIDs.GetItemID();
            }

            public override string ToString()
            {
                return "ID: " + this.id + ", Name: " + this.name + ", Category: " + this.category.Getname() + ", ex. Date: " + this.exDate.ToString("'Min: 'm', Hour: 'H', Day: 'd', Month: 'M', Year: 'yyyy") + ", Size: " + this.Size;
            }

            public bool IsIdentical(Item otherItem)
            {
                if (otherItem == null) return false;
                if (this.id == otherItem.id)
                {
                    return true;
                }
                return false;
            }

            public void Open()
            {
                this.exDate = this.exDate.AddDays(-(this.category.Getexdays()));
            }

            public void Setname(string name)
            {
                this.name = name;
            }

            public void Setcategory(Category category)
            {
                this.category = category;
            }

            public void SetexDate(DateTime exDate)
            {
                if (exDate == default) throw new ArgumentException("exDate must be set");
                this.exDate = exDate;
            }
            public void SetSize(double Size)
            {
                if (Size <= 0) throw new ArgumentException("Size must be greater than 0");
                this.Size = Size;
            }

            public void SetdateOfThrowing(DateTime dateOfThrowing)
            {
                this.dateOfThrowing = dateOfThrowing;
            }

            public string Getname()
            {
                return this.name;
            }
            public int GetID()
            {
                return this.id;
            }

            public Category Getcategory()
            {
                return this.category;
            }

            public DateTime GetexDate()
            {
                return this.exDate;
            }
            public double GetSize()
            {
                return this.Size;
            }

            public DateTime GetdateOfThrowing()
            {
                return this.dateOfThrowing;
            }
        }

        public static void PrintItems(List<Item> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null) Console.WriteLine(items[i].ToString());
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
            List<Item> badItems = fridge.Find(item => item.GetexDate() < DateTime.Now);
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
            compartments.Add(new Compartment("Freezer", 20, 0, new List<int> { 3 }, 0));
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
            PrintItems(fridge3.GetThrownThisMonth());
        }
    }
}