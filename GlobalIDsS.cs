using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeTron
{
    public class GlobalIDs
    {
        private static int ItemID = 0;
        private static int CompartmentID = 0;
        private static int CategoryID = 0;
        private static List<string> categories = new List<string>();
        private static List<Category> categories_obj = new List<Category>();
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
            List<string> returncategories = new List<string>();
            for (int i = 0; i < categories.Count; i++) returncategories.Add(categories[i]);
            return returncategories;
        }

        public static List<Category> Getcategories_obj()
        {
            List<Category> returncategories = new List<Category>();
            for (int i = 0; i < categories_obj.Count; i++) returncategories.Add(categories_obj[i]);
            return returncategories;
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

        public static bool Addcategory(Category category)
        {
            if (category == null || categories.Contains(category.Getname())) return false;
            categories_obj.Add(category);
            categories.Add(category.Getname());
            return true;
        }

        public static bool Removecategory(Category category)
        {
            if (category == null) return false;
            if (!categories.Contains(category.Getname()) || !categories_obj.Contains(category)) return false;
            categories_obj.Remove(category);
            categories.Remove(category.Getname());
            return true;
        }
        public static bool Removecategory(string category) {
            if (category == null) return false;
            if (!categories.Contains(category)) return false;
            bool okay = false;
            for (int i = 0; i < categories_obj.Count; i++)
            {
                if (categories_obj[i] != null && categories_obj[i].Getname().Equals(category))
                {
                    okay = true;
                    categories_obj.RemoveAt(i);
                }
            }
            if (okay)
            {
                categories.Remove(category);
                return true;
            } return false;

        }
    }
}
