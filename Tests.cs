using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using static FridgeTron.Program;

namespace FridgeTron.Tests
{
    [TestClass]
    public class FridgeTests
    {
        static Category fruits = new Category("Fruits", 3, 2);
        static Item NewItem(string name, int days = 10) => new Item(name, fruits, DateTime.Now.AddDays(days), 1);

        [TestMethod]
        public void AddNewItem_RejectsDuplicate()
        {
            var fridge = new Fridge(3, 1, 100);
            var item = NewItem("Banana");
            fridge.AddNewItem(item);
            Assert.IsFalse(fridge.AddNewItem(item));
        }

        [TestMethod]
        public void SortByExDate_ReturnsEarliestFirst()
        {
            var fridge = new Fridge(3, 1, 100);
            var late = NewItem("Late", 30);
            var early = NewItem("Early", 5);
            fridge.AddNewItem(late);
            fridge.AddNewItem(early);
            Assert.AreEqual(early, fridge.SortByExDate()[0]);
        }

        [TestMethod]
        public void RemoveItemByName_RemovesClosestExpiry()
        {
            var fridge = new Fridge(3, 1, 100);
            var far = NewItem("Banana", 30);
            var near = NewItem("Banana", 5);
            fridge.AddNewItem(far);
            fridge.AddNewItem(near);
            fridge.RemoveItem("Banana");
            Assert.AreEqual(far, fridge.ListAll()[0]);
        }

        [TestMethod]
        public void GetThrownThisMonth_ContainsRemovedItem()
        {
            var fridge = new Fridge(3, 1, 100);
            var old = NewItem("Old", -1);
            fridge.AddNewItem(old);
            fridge.RemoveBy(i => i.GetexDate() < DateTime.Now);
            CollectionAssert.Contains(fridge.GetThrownThisMonth(), old);
        }

        [TestMethod]
        public void AddItemToCompartment_ReturnsFalse_WhenCategoryNotAllowed()
        {
            var frozen = new Category("Frozen", 0, 0);
            var comp = new Compartment("Freezer", 20, 0, new List<int> { frozen.Gettype() }, 0);
            var fridge = new Fridge(3, 0, 100, new List<Compartment> { comp });
            Assert.IsFalse(fridge.AddItemToCompartment(NewItem("Banana"), comp.GetID()));
        }
    }
}