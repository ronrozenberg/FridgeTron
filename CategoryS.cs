using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static FridgeTron.Program;

namespace FridgeTron
{
    public class Category
    {
        private string name;
        private int exdays;
        private double neededTemp;

        public Category(string name, int exdays, double neededTemp)
        {
            this.name = name;
            this.exdays = exdays;
            this.neededTemp = neededTemp;
            this.CheckCategory();
        }

        public string Getname()
        {
            return this.name;
        }

        public int Getexdays()
        {
            return this.exdays;
        }

        public double GetneededTemp()
        {
            return this.neededTemp;
        }

        public void CheckCategory()
        {
            if (!GlobalIDs.Addcategory(this)) throw new Exception("Category Exists");
        }

    }
}
