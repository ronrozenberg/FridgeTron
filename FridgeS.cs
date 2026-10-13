using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static FridgeTron.Program;

namespace FridgeTron
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
            if (compartments != null) this.compartments = compartments;
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
            if (this.itemList[index].GetexDate() < GetCurrDate() && !CheckDupe(this.itemList[index], this.thrownThisMonth))
            {
                this.thrownThisMonth.Add(this.itemList[index]);
                thrownThisMonth[thrownThisMonth.Count - 1].SetdateOfThrowing(GetCurrDate());
            }
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
                    if (item.GetexDate() < GetCurrDate() && !CheckDupe(item, this.thrownThisMonth))
                    {
                        this.thrownThisMonth.Add(item);
                        thrownThisMonth[thrownThisMonth.Count - 1].SetdateOfThrowing(GetCurrDate());
                    }
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
            for (int i = 0; i < sortedItems.Count - 1; i++)
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
                    if (this.itemList[i].GetexDate() < GetCurrDate() && !CheckDupe(this.itemList[i], this.thrownThisMonth))
                    {
                        this.thrownThisMonth.Add(this.itemList[i]);
                        thrownThisMonth[thrownThisMonth.Count - 1].SetdateOfThrowing(GetCurrDate());
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
            for (int i = 0; i < this.compartments.Count; i++)
            {
                if (this.compartments[i] != null && this.compartments[i].GetID() == id)
                {
                    if (compartments[i].AddItem(item)) return true;
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
                if (this.thrownThisMonth[i] == null || (GetCurrDate().Month != this.thrownThisMonth[i].GetdateOfThrowing().Month))
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
}
