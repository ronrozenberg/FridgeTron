using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FridgeTron.Program;

namespace FridgeTron
{
    public class Item
    {
        private string name;
        private Category category;
        private DateTime exDate;
        private double Size;
        private int id;
        private DateTime dateOfThrowing;
        private DateTime dateOfFreezing;
        private string stage;

        public Item(string name, string category, int exdays, double temp, DateTime exDate, double Size)
        {
            if (Size <= 0) throw new ArgumentException("Size must be greater than 0");
            if (exDate == default) throw new ArgumentException("exDate must be set");
            this.name = name;
            if(GlobalIDs.GetCategoryType(category) == -1) GlobalIDs.Addcategory(new Category(category, exdays, temp));
            this.category = new Category(category, exdays, temp);
            this.exDate = exDate;
            this.Size = Size;
            this.id = GlobalIDs.GetItemID();
            if (temp > 0) this.stage = "original";
            else this.stage = "frozen";
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
            if (category.GetneededTemp() > 0) this.stage = "original";
            else this.stage = "frozen";
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

        public string GetStage()
        {
            return this.stage;
        }

        public void SetStage(string stage)
        {
            if (!stage.Equals("original") && !stage.Equals("frozen") && !stage.Equals("thawed")) throw new ArgumentException("illegal stage");
            if(stage.Equals("frozen")) this.dateOfFreezing = DateTime.Now;
            if (stage.Equals("thawed"))
            {
                if (this.exDate.Subtract(this.dateOfFreezing).Days >= this.category.Getexdays()) this.exDate = DateTime.Now.AddDays(this.category.Getexdays());
                else this.exDate = DateTime.Now.Add(this.exDate.Subtract(this.dateOfFreezing));
            }
            this.stage = stage;
        }

        public DateTime GetdateOfThrowing()
        {
            return this.dateOfThrowing;
        }
    }
}
