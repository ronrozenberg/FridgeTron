namespace FridgeTron
{
    internal class Program
    {
        public class Fridge
        {
            private int maxItems;
            private double currentTemp;
            private Item[] arr;
            public Fridge(int maxItems, double currentTemp)
            {
<<<<<<< HEAD
=======
                if(maxItems <= 0) throw new ArgumentException("maxItems must be greater than 0");
>>>>>>> 2d325eb (fixes to stage 1)
                this.maxItems = maxItems;
                this.currentTemp = currentTemp;
                this.arr = new Item[maxItems];
            }
            public bool AddNewItem(Item item)
            {
<<<<<<< HEAD
                if(this.CountItems()>= this.maxItems) return false;
=======
                if (item == null) return false;
                if (this.CountItems()>= this.maxItems) return false;
                for (int i = 0; i < this.arr.Length; i++)
                {
                    if (this.arr[i] != null && this.arr[i] == item) return false;
                }
>>>>>>> 2d325eb (fixes to stage 1)
                for (int i = 0; i < this.arr.Length; i++)
                {
                    if (this.arr[i] == null)
                    {
                        this.arr[i] = item;
                        return true;
                    }
                }
                return false;
            }
            public int CountItems()
            {
                int count = 0;
                for (int i = 0; i < this.arr.Length; i++)
                {
                    if (this.arr[i] != null) count++;
                }
                return count;
            }
            public bool RemoveItem(int index)
            {
<<<<<<< HEAD
                for (int i = 0; i < this.arr.Length; i++)
                {
                    if (i == index)
                    {
                        this.arr[i] = null;
                        return true;
                    }
                }
                return false;
            }
            public void ListAll()
            {
                for (int i = 0; i < this.arr.Length; i++)
                {
                    if (this.arr[i] != null) Console.WriteLine(this.arr[i].ToString());
                }
            }
            public void ListBad()
            {
                for (int i = 0; i < this.arr.Length; i++)
                {
                    if (this.arr[i] != null && this.arr[i].GetexDate().IsExpired(GetCurrDate()))
                        Console.WriteLine(this.arr[i].ToString());
                }
            }
            public void RemoveBad()
            {
                for (int i = 0; i < this.arr.Length; i++)
                {
                    if (this.arr[i] != null && this.arr[i].GetexDate().IsExpired(GetCurrDate()))
=======
                if(index < 0 || index >= this.arr.Length) return false;
                if (this.arr[index] == null) return false;
                arr[index] = null;
                return true;
            }
            public Item[] ListAll()
            {
                return this.arr;
            }
            public Item[] Find(Func<Item,bool> condition)
            {
                Item[] foundItems = new Item[this.arr.Length];
                int count = 0;
                for (int i = 0; i < this.arr.Length; i++)
                {
                    if (this.arr[i] != null && condition(this.arr[i]))
                    {
                        foundItems[count] = this.arr[i];
                        count++;
                    }
                }
                return foundItems;
            }
            public void RemoveBy(Func<Item, bool> condition)
            {
                for (int i = 0; i < this.arr.Length; i++)
                {
                    if (this.arr[i] != null && condition(this.arr[i]))
>>>>>>> 2d325eb (fixes to stage 1)
                        this.arr[i] = null;
                }
            }

            public void SetmaxItems(int num)
            {
<<<<<<< HEAD
                this.maxItems = num;
            }
=======
                Item[] newarr = new Item[num];
                if(num > this.maxItems)
                {
                    for (int i = 0; i < this.arr.Length; i++)
                    {
                        newarr[i] = this.arr[i];
                    }
                }
                else
                {
                    for (int i = 0; i < num; i++)
                    {
                        newarr[i] = this.arr[i];
                    }
                }
                this.maxItems = num;
                this.arr = newarr;
            }

>>>>>>> 2d325eb (fixes to stage 1)
            public void SetcurrentTemp(double num)
            {
                this.currentTemp = num;
            }
            public int GetmaxItems()
            {
                return this.maxItems;
            }
            public double GetcurrentTemp()
            {
                return this.currentTemp;
            }
        }
        public class Item
        {
            private string name;
            private string category;
            private Date exDate;
            public Item(string name, string category, Date exDate)
            {
                this.name = name;
                this.category = category;
                this.exDate = exDate;
            }
            public override string ToString()
            {
                return "Name: " + this.name + ", Category: " + this.category + ", ex. Date: " + this.exDate.ToString();
            }
            public void Setname(string name)
            {
                this.name = name;
            }
            public void Setcategory(string category)
            {
                this.category = category;
            }
            public void SetexDate(Date exDate)
            {
                this.exDate = exDate;
            }
            public string Getname()
            {
                return this.name;
            }
            public string Getcategory()
            {
                return this.category;
            }
            public Date GetexDate()
            {
                return this.exDate;
            }
        }
        public class Date
        {
            private int minutes;
            private int hours;
            private int day;
            private int month;
            private int year;

            public Date(int minutes, int hours, int day, int month, int year)
            {
<<<<<<< HEAD
                this.minutes = minutes;
                this.hours = hours;
                this.day = day;
=======
                if (minutes < 0 || minutes > 59) throw new ArgumentException("Minutes must be between 0 and 59");
                this.minutes = minutes;
                if (hours < 0 || hours > 23) throw new ArgumentException("Hours must be between 0 and 23");
                this.hours = hours;
                if (day < 1 || day > 31) throw new ArgumentException("Day must be between 1 and 31");
                this.day = day;
                if (month < 1 || month > 12) throw new ArgumentException("Month must be between 1 and 12");
>>>>>>> 2d325eb (fixes to stage 1)
                this.month = month;
                this.year = year;
            }

            public bool IsExpired(Date currentDate)
            {
                if (this.GetMonths() > currentDate.GetMonths()) return false;
                if (this.GetMonths() == currentDate.GetMonths())
                {
                    if (this.day == currentDate.GetDay())
                    {
                        if (this.GetHourMinutes() >= currentDate.GetHourMinutes()) return false;
                    }
                    else if (this.day > currentDate.GetDay()) return false;
                }
                return true;
            }

            public override string ToString()
            {
                return "Min: " + this.minutes + ", Hour: " + this.hours + ", Day: " + this.day + ", Month: " + this.month + ", Year: " + this.year;
            }

            public void Setminutes(int minutes) { this.minutes = minutes; }
            public void Setday(int day) { this.day = day; }
            public void Sethours(int hours) { this.hours = hours; }
            public void Setmonths(int month) { this.month = month; }
            public void Setyear(int year) { this.year = year; }
            public int GetMonths() { return this.year * 12 + this.month; }
            public int GetHourMinutes() { return this.hours * 60 + this.minutes; }
            public int GetDay() { return this.day; }
        }
        public static Date GetCurrDate()
        {
            DateTime now = DateTime.Now;
            return new Date(now.Minute, now.Hour, now.Day, now.Month, now.Year);
        }
<<<<<<< HEAD
=======
        public static void PrintItems(Item[] items)
        {
            for(int i = 0; i < items.Length; i++)
            {
                if (items[i] != null) Console.WriteLine(items[i].ToString());
            }
        }
>>>>>>> 2d325eb (fixes to stage 1)
        public static void Main(string[] args)
        {
            Item banana = new Item("Banana", "Fruits", new Date(12, 12, 12, 12, 2012));
            Item strawberry = new Item("Strawberry", "Fruits", new Date(12, 12, 12, 12, 2027));
            Item butter = new Item("Butter", "Dairy", new Date(12, 12, 12, 12, 2026));
            Fridge fridge = new Fridge(3, 4);
            fridge.AddNewItem(banana);
            fridge.AddNewItem(strawberry);
            fridge.AddNewItem(butter);
            Console.WriteLine("All:");
<<<<<<< HEAD
            fridge.ListAll();
            Console.WriteLine("Bad:");
            fridge.ListBad();
            Console.WriteLine("Adding another banana will print false:");
            Console.WriteLine(fridge.AddNewItem(banana));
            Console.WriteLine("Removing Bad...");
            fridge.RemoveBad();
            Console.WriteLine("All:");
            fridge.ListAll();
=======
            PrintItems(fridge.ListAll());
            Console.WriteLine("Bad:");
            Item[] badItems = fridge.Find(item => item.GetexDate().IsExpired(GetCurrDate()));
            PrintItems(badItems);
            Console.WriteLine("Adding another banana will print false:");
            Console.WriteLine(fridge.AddNewItem(banana));
            Console.WriteLine("Removing Bad...");
            fridge.RemoveBy(item => item.GetexDate().IsExpired(GetCurrDate()));
            Console.WriteLine("All:");
            PrintItems(fridge.ListAll());
            Console.WriteLine("Extending fridge from 3->4...");
            fridge.SetmaxItems(4);
            PrintItems(fridge.ListAll());
            Console.WriteLine("Adding another banana will print true:");
            Console.WriteLine(fridge.AddNewItem(banana));
            Item pear = new Item("Pear", "Fruits", new Date(12, 12, 12, 12, 2012));
            Console.WriteLine("Adding a pear will print true:");
            Console.WriteLine(fridge.AddNewItem(pear));
            Console.WriteLine("Adding another banana will print false:");
            Console.WriteLine(fridge.AddNewItem(banana));
            PrintItems(fridge.ListAll());
>>>>>>> 2d325eb (fixes to stage 1)
        }
    }
}
