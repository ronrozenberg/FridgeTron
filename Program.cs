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
                this.maxItems = maxItems;
                this.currentTemp = currentTemp;
                this.arr = new Item[maxItems];
            }
            public bool AddNewItem(Item item)
            {
                if(this.CountItems()>= this.maxItems) return false;
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
                        this.arr[i] = null;
                }
            }

            public void SetmaxItems(int num)
            {
                this.maxItems = num;
            }
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
                this.minutes = minutes;
                this.hours = hours;
                this.day = day;
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
            fridge.ListAll();
            Console.WriteLine("Bad:");
            fridge.ListBad();
            Console.WriteLine("Adding another banana will print false:");
            Console.WriteLine(fridge.AddNewItem(banana));
            Console.WriteLine("Removing Bad...");
            fridge.RemoveBad();
            Console.WriteLine("All:");
            fridge.ListAll();
        }
    }
}
