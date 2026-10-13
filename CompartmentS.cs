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
            if (!this.allowedCategories.Contains(GlobalIDs.GetCategoryType(item.Getcategory().Getname()))) return false;
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
}
