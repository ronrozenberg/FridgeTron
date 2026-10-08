namespace FridgeTron
{
    internal class Program
    {
        public class Fridge
        {
            private int maxItems;
            private double currentTemp;
            private Item[] arr;
            private double maxWeight;

            public Fridge(int maxItems, double currentTemp, double maxWeight)
            {
                if (maxItems <= 0) throw new ArgumentException("maxItems must be greater than 0");
                if (maxWeight <= 0) throw new ArgumentException("maxWeight must be greater than 0");
                this.maxItems = maxItems;
                this.currentTemp = currentTemp;
                this.arr = new Item[maxItems];
                this.maxWeight = maxWeight;
            }

            public bool AddNewItem(Item item)
            {
                if (item == null) return false;
                if (this.CountItems() >= this.maxItems) return false;
                if (this.SumWeight() + item.Getweight() > this.maxWeight) return false;
                if (this.CheckDupe(item)) return false;
                if (!this.CheckItem(item)) return false;
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

            public double SumWeight()
            {
                double sum = 0;
                for (int i = 0; i < this.arr.Length; i++)
                {
                    if (this.arr[i] != null) sum += this.arr[i].Getweight();
                }
                return sum;
            }

            public bool CheckDupe(Item item)
            {
                if (item == null) return false;
                for (int i = 0; i < this.arr.Length; i++)
                {
                    if (this.arr[i] != null && (this.arr[i] == item || this.arr[i].IsIdentical(item))) return true;
                }
                return false;
            }
            public bool CheckItem(Item item)
            {
                if (item == null) return false;
                if (this.currentTemp <= 0)
                {
                    string[] allowedCategories = {"Ice"};
                    for(int i = 0; i < allowedCategories.Length; i++)
                    {
                        if (item.Getcategory() == allowedCategories[i]) return true;
                    }
                }
                else
                {
                    string[] allowedCategories = { "Fruits", "Dairy" };
                    for (int i = 0; i < allowedCategories.Length; i++)
                    {
                        if (item.Getcategory() == allowedCategories[i]) return true;
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
                if (index < 0 || index >= this.arr.Length) return false;
                if (this.arr[index] == null) return false;
                arr[index] = null;
                return true;
            }

            public bool RemoveItem(Item item)
            {
                if (item == null) return false;
                for (int i = 0; i < this.arr.Length; i++)
                {
                    if (this.arr[i] != null && this.arr[i] == item)
                    {
                        arr[i] = null;
                        return true;
                    }
                }
                return false;
            }

            public bool RemoveItem(string item)
            {
                if (item == null) return false;
                Item[] foundItems = this.Find(i => i.Getname() == item);
                int smallestIndex = 0;
                if (foundItems.Length == 0) return false;
                if (foundItems.Length == 1)
                {
                    RemoveItem(foundItems[0]);
                    return true;
                }
                if (foundItems.Length > 1)
                {
                    Date closestDate = foundItems[0].GetexDate();
                    for (int i = 0; i < foundItems.Length; i++)
                    {
                        if (foundItems[i] != null && closestDate.Compare(foundItems[i].GetexDate()) == true)
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

            public Item[] ListAll()
            {
                Item[] result = new Item[this.CountItems()];
                int count = 0;

                for (int i = 0; i < this.arr.Length; i++)
                {
                    if (this.arr[i] != null)
                    {
                        result[count] = this.arr[i];
                        count++;
                    }
                }

                return result;
            }

            public Item[] Find(Func<Item, bool> condition)
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
                Item[] result = new Item[count];
                for (int i = 0; i < count; i++)
                {
                    result[i] = foundItems[i];
                }
                return result;
            }

            public void RemoveBy(Func<Item, bool> condition)
            {
                for (int i = 0; i < this.arr.Length; i++)
                {
                    if (this.arr[i] != null && condition(this.arr[i]))
                        this.arr[i] = null;
                }
            }

            public void SetmaxItems(int num)
            {
                if (num <= 0) throw new ArgumentException("maxItems must be greater than 0");
                Item[] newarr = new Item[num];
                bool placed = false;
                if (num > this.maxItems)
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
                    for (int i = num; i < this.arr.Length; i++)
                    {
                        if (this.arr[i] != null)
                        {
                            placed = false;
                            for (int j = 0; j < newarr.Length; j++)
                            {
                                if (newarr[j] == null && !placed)
                                {
                                    newarr[j] = this.arr[i];
                                    placed = true;
                                }
                            }
                        }
                    }
                }
                this.maxItems = num;
                this.arr = newarr;
            }

            public void SetmaxWeight(double num)
            {
                if (num <= 0) throw new ArgumentException("maxWeight must be greater than 0");
                this.maxWeight = num;
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

            public double GetmaxWeight()
            {
                return this.maxWeight;
            }
        }
        public class Item
        {
            private string name;
            private string category;
            private Date exDate;
            private double weight;

            public Item(string name, string category, Date exDate, double weight)
            {
                if (weight <= 0) throw new ArgumentException("Weight must be greater than 0");
                if (exDate == null) throw new ArgumentException("exDate must not be null");
                this.name = name;
                this.category = category;
                this.exDate = exDate;
                this.weight = weight;
            }

            public override string ToString()
            {
                return "Name: " + this.name + ", Category: " + this.category + ", ex. Date: " + this.exDate.ToString() + ", Weight: " + this.weight;
            }

            public bool IsIdentical(Item otherItem)
            {
                if (otherItem == null) return false;
                if (this.name == otherItem.Getname() && this.category == otherItem.Getcategory() && this.exDate.IsSame(otherItem.GetexDate()) == true && this.weight == otherItem.Getweight())
                {
                    return true;
                }
                return false;
            }

            public void Open()
            {
                if (this.category == "Dairy")
                {
                    this.exDate.ChangeDaysBy(-7);
                }
                else if (this.category == "Fruits")
                {
                    this.exDate.ChangeDaysBy(-3);
                }
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
                if (exDate == null) throw new ArgumentException("exDate must not be null");
                this.exDate = exDate;
            }
            public void Setweight(double weight)
            {
                if (weight <= 0) throw new ArgumentException("Weight must be greater than 0");
                this.weight = weight;
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
            public double Getweight()
            {
                return this.weight;
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
                if (minutes < 0 || minutes > 59) throw new ArgumentException("Minutes must be between 0 and 59");
                this.minutes = minutes;
                if (hours < 0 || hours > 23) throw new ArgumentException("Hours must be between 0 and 23");
                this.hours = hours;
                if (day < 1 || day > 31) throw new ArgumentException("Day must be between 1 and 31");
                this.day = day;
                if (month < 1 || month > 12) throw new ArgumentException("Month must be between 1 and 12");
                this.month = month;
                this.year = year;
            }

            public bool Compare(Date otherDate)
            {
                if (this.GetMonths() > otherDate.GetMonths()) return true;
                if (this.GetMonths() == otherDate.GetMonths())
                {
                    if (this.day == otherDate.GetDay())
                    {
                        if (this.GetHourMinutes() > otherDate.GetHourMinutes()) return true;
                    }
                    else if (this.day > otherDate.GetDay()) return true;
                }
                return false;
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

            public bool IsSame(Date otherDate)
            {
                if (this.GetMonths() == otherDate.GetMonths())
                {
                    if (this.day == otherDate.GetDay())
                    {
                        if (this.GetHourMinutes() == otherDate.GetHourMinutes()) return true;
                    }
                }
                return false;
            }
            public void ChangeDaysBy(int days)
            {
                if (days < 0) {
                    if (this.day <= Math.Abs(days))
                    {
                        this.day = 31 - (Math.Abs(days) - this.day);
                        if (this.month == 1)
                        {
                            this.year = this.year - 1;
                            this.month = 12;
                        }
                        else this.month = this.month - 1;
                    }
                    else
                    {
                        this.day = this.day + days;
                    }
                } else if(days > 0)
                {
                    if (this.day + days > 31)
                    {
                        this.day = (this.day + days) - 31;
                        if (this.month == 12)
                        {
                            this.year = this.year + 1;
                            this.month = 1;
                        }
                        else this.month = this.month + 1;
                    }
                    else
                    {
                        this.day = this.day + days;
                    }
                }
            }

            public override string ToString()
            {
                return "Min: " + this.minutes + ", Hour: " + this.hours + ", Day: " + this.day + ", Month: " + this.month + ", Year: " + this.year;
            }

            public void Setminutes(int minutes) {
                if (minutes < 0 || minutes > 59) throw new ArgumentException("Minutes must be between 0 and 59");
                this.minutes = minutes;
            }
            public void Setday(int day) {
                if (day < 1 || day > 31) throw new ArgumentException("Day must be between 1 and 31");
                this.day = day;
            }
            public void Sethours(int hours) {
                if (hours < 0 || hours > 23) throw new ArgumentException("Hours must be between 0 and 23");
                this.hours = hours; 
            }
            public void Setmonths(int month) {
                if (month < 1 || month > 12) throw new ArgumentException("Month must be between 1 and 12");
                this.month = month;
            }
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

        public static void PrintItems(Item[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null) Console.WriteLine(items[i].ToString());
            }
        }

        public static void Main(string[] args)
        {
            // level 1
            Console.WriteLine("=======level 1=======");
            Item banana = new Item("Banana", "Fruits", new Date(12, 12, 12, 12, 2012), 12);
            Item strawberry = new Item("Strawberry", "Fruits", new Date(12, 12, 12, 12, 2027), 5);
            Item butter = new Item("Butter", "Dairy", new Date(12, 12, 12, 12, 2026), 11);
            Fridge fridge = new Fridge(3, 4, 100);
            fridge.AddNewItem(banana);
            fridge.AddNewItem(strawberry);
            fridge.AddNewItem(butter);
            Console.WriteLine("All:");
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
            Item pear = new Item("Pear", "Fruits", new Date(12, 12, 12, 12, 2012), 12);
            Console.WriteLine("Adding a pear will print true:");
            Console.WriteLine(fridge.AddNewItem(pear));
            Console.WriteLine("Adding another banana will print false:");
            Console.WriteLine(fridge.AddNewItem(banana));
            PrintItems(fridge.ListAll());

            //level 2 part 1
            Console.WriteLine("=======level 2=======");
            Item a = new Item("Banana", "Fruits", new Date(12, 12, 12, 11, 2026), 12);
            Item b = new Item("Strawberry", "Fruits", new Date(12, 12, 12, 12, 2027), 5);
            Item c = new Item("Banana", "Fruits", new Date(12, 12, 12, 12, 2026), 12);

            Fridge fridge2 = new Fridge(3, 4, 100);
            fridge2.AddNewItem(a);
            fridge2.AddNewItem(b);
            fridge2.AddNewItem(c);
            PrintItems(fridge2.ListAll());

            Console.WriteLine("Removing by 'Banana' will remove the one whose date's is closer");
            fridge2.RemoveItem("Banana");
            PrintItems(fridge2.ListAll());
            //level 2 part 2
            Fridge freezer = new Fridge(3, -18, 100);
            Item ice = new Item("Ice", "Ice", new Date(12, 12, 12, 12, 2027), 5);
            Console.WriteLine("Adding ice to freezer will print true:");
            Console.WriteLine(freezer.AddNewItem(ice));
            Console.WriteLine("Adding ice to fridge will print false:");
            Console.WriteLine(fridge2.AddNewItem(ice));
            Console.WriteLine("Adding too much weight will print false:");
            Item heavyItem = new Item("Watermelon", "Fruits", new Date(12, 12, 12, 12, 2027), 1000);
            Console.WriteLine(fridge2.AddNewItem(heavyItem));
            Console.WriteLine("Adding a warm item will print false:");
            Console.WriteLine(freezer.AddNewItem(butter));
            Console.WriteLine("Freezer:");
            PrintItems(freezer.ListAll());
            Console.WriteLine("Fridge:");
            PrintItems(fridge2.ListAll());
            Console.WriteLine("Weight sum of both:");
            Console.WriteLine(freezer.SumWeight() + fridge2.SumWeight());
        }
    }
}