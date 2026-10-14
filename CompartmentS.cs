using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FridgeTron.Program;

namespace FridgeTron
{
    public class Compartment
    {
        private string name;
        private int type;
        private int maxItems;
        private double size;
        private int id;
        private double currentTemp;
        private List<int> allowedCategories;
        private List<Item> items = new List<Item>();

        public Compartment(string name, double size, int type, List<int> allowedCategories, double currentTemp, int maxItems)
        {
            this.name = name;
            this.size = size;
            this.id = GlobalIDs.GetCompartmentID();
            this.type = type;
            this.allowedCategories = allowedCategories;
            this.currentTemp = currentTemp;
            this.maxItems = maxItems;
        }

        public void RemoveBy(Func<Item, bool> condition)
        {
            for (int i = 0; i < this.items.Count; i++)
            {
                if (this.items[i] != null && condition(this.items[i]))
                {
                    if (this.items[i].GetexDate() < GetCurrDate() && !this.CheckDupe(this.items[i], GlobalIDs.GetThrownThisMonth()))
                    {
                        GlobalIDs.GetThrownThisMonthList().Add(this.items[i]);
                        GlobalIDs.GetThrownThisMonthList()[GlobalIDs.GetThrownThisMonthList().Count - 1].SetdateOfThrowing(GetCurrDate());
                    }
                    this.items.RemoveAt(i);
                }
            }
        }

        public bool RemoveItem(int index)
        {
            if (index < 0 || index >= this.items.Count) return false;
            if (this.items[index] == null) return false;
            if (this.items[index].GetexDate() < GetCurrDate() && !CheckDupe(this.items[index], GlobalIDs.GetThrownThisMonth()))
            {
                GlobalIDs.GetThrownThisMonthList().Add(this.items[index]);
                GlobalIDs.GetThrownThisMonthList()[GlobalIDs.GetThrownThisMonthList().Count - 1].SetdateOfThrowing(GetCurrDate());
            }
            this.items.RemoveAt(index);
            return true;
        }

        public bool RemoveItem(Item item)
        {
            if (item == null) return false;
            for (int i = 0; i < this.items.Count; i++)
            {
                if (this.items[i] != null && this.items[i] == item)
                {
                    if (item.GetexDate() < GetCurrDate() && !CheckDupe(item, GlobalIDs.GetThrownThisMonthList()))
                    {
                        GlobalIDs.GetThrownThisMonthList().Add(item);
                        GlobalIDs.GetThrownThisMonthList()[GlobalIDs.GetThrownThisMonthList().Count - 1].SetdateOfThrowing(GetCurrDate());
                    }
                    this.items.RemoveAt(i);
                    return true;
                }
            }
            return false;
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
            if (!this.allowedCategories.Contains(GlobalIDs.GetCategoryType(item.Getcategory().Getname()))) return false;
            if (item.Getcategory().GetneededTemp() < this.currentTemp) return false;
            if (item.GetStage().Equals("thawed") && this.currentTemp <= 0) return false;
            if (this.SumSize() + item.GetSize() > this.size) throw new ArgumentException("compartment size exceeded");
            this.items.Add(item);
            if (item.GetStage().Equals("original") && this.currentTemp <= 0) item.SetStage("frozen");
            if (item.GetStage().Equals("frozen") && this.currentTemp > 0) item.SetStage("thawed");
            return true;
        }

        public int CountItems()
        {
            int count = 0;
            for (int i = 0; i < this.items.Count; i++)
            {
                if (this.items[i] != null) count++;
            }
            return count;
        }
        public void SetmaxItems(int num)
        {
            if (num <= 0) throw new ArgumentException("maxItems must be greater than 0");
            List<Item> newitemList = new List<Item>();

            if (num < this.maxItems)
            {
                for (int i = 0; i < num; i++)
                {
                    newitemList.Add(this.items[i]);
                }
                this.items = newitemList;
            }
            this.maxItems = num;
        }


        public double GetcurrentTemp()
        {
            return this.currentTemp;
        }

        public int GetmaxItems()
        {
            return this.maxItems;
        }

        public double GetmaxSize()
        {
            return this.size;
        }
        public List<Item> GetItems()
        {
            return this.items;
        }

        public int GetID()
        {
            return this.id;
        }

        public void SetmaxSize(double num)
        {
            if (num <= 0) throw new ArgumentException("maxSize must be greater than 0");
            if (num < this.SumSize()) throw new ArgumentException("maxSize must be greater than current size");
            this.size = num;
        }

        public void SetcurrentTemp(double num)
        {
            this.currentTemp = num;
        }
    }
}
