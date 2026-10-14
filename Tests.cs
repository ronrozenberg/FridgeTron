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
        static Category icy = new Category("Icy", 0, 0);
        static Category meltable = new Category("Meltable", 2, 24);

        Compartment freezer, warmer;
        Fridge fridge;

        [TestInitialize]
        public void Setup()
        {
            GlobalIDs.GetThrownThisMonthList().Clear();

            var allowed = new List<int>
            {
                GlobalIDs.GetCategoryType("Meltable")
            };

            freezer = new Compartment(
                "Freezer", 20, 0, allowed, 0, 3);

            warmer = new Compartment(
                "Warmer", 20, 0, allowed, 24, 3);

            fridge = new Fridge(
                10, -18, 100,
                new List<Compartment> { freezer, warmer });
        }

        static Item NewItem(string name, int days = 10)
            => new Item(name, fruits,
                DateTime.Now.AddDays(days), 1);

        static Item NewIceCream(int days = 3, string name = "Ice cream")
            => new Item(name, meltable,
                DateTime.Now.AddDays(days), 5);

        Item AddTo(Compartment c, Item item = null)
        {
            item = item ?? NewIceCream();

            Assert.IsTrue(
                fridge.AddItemToCompartment(item, c.GetID()));

            return item;
        }

        bool Move(Item item, Compartment from, Compartment to)
            => fridge.MoveItemToCompartment(
                item, from.GetID(), to.GetID());

        [TestMethod]
        public void AddNewItem_RejectsDuplicate()
        {
            var testFridge = new Fridge(3, 1, 100);
            var item = NewItem("Banana");

            Assert.IsTrue(testFridge.AddNewItem(item));
            Assert.IsFalse(testFridge.AddNewItem(item));
        }

        [TestMethod]
        public void SortByExDate_ReturnsEarliestFirst()
        {
            var testFridge = new Fridge(3, 1, 100);
            var late = NewItem("Late", 30);
            var early = NewItem("Early", 5);

            testFridge.AddNewItem(late);
            testFridge.AddNewItem(early);

            Assert.AreEqual(early, testFridge.SortByExDate()[0]);
        }

        [TestMethod]
        public void RemoveItemByName_RemovesClosestExpiry()
        {
            var testFridge = new Fridge(3, 1, 100);
            var far = NewItem("Banana", 30);
            var near = NewItem("Banana", 5);

            testFridge.AddNewItem(far);
            testFridge.AddNewItem(near);
            testFridge.RemoveItem("Banana");

            Assert.AreEqual(far, testFridge.ListAll()[0]);
        }

        [TestMethod]
        public void GetThrownThisMonth_ContainsRemovedItem()
        {
            var testFridge = new Fridge(3, 1, 100);
            var old = NewItem("Old", -1);

            testFridge.AddNewItem(old);
            testFridge.RemoveBy(i => i.GetexDate() < DateTime.Now);

            CollectionAssert.Contains(
                GlobalIDs.GetThrownThisMonth(), old);
        }

        [TestMethod]
        public void AddItemToCompartment_ReturnsFalse_WhenCategoryNotAllowed()
        {
            var allowed = new List<int>
            {
                GlobalIDs.GetCategoryType("Meltable")
            };

            var comp = new Compartment(
                "Restricted", 20, 0, allowed, 24, 3);

            var testFridge = new Fridge(
                3, 0, 100, new List<Compartment> { comp });

            Assert.IsFalse(
                testFridge.AddItemToCompartment(
                    NewItem("Banana"), comp.GetID()));
        }

        [TestMethod]
        public void AddingToFreezer_Freezes()
        {
            var item = AddTo(freezer);
            Assert.AreEqual("frozen", item.GetStage());
        }

        [TestMethod]
        public void MovingToFreezer_Freezes()
        {
            var item = AddTo(warmer);

            Assert.IsTrue(Move(item, warmer, freezer));
            Assert.AreEqual("frozen", item.GetStage());
        }

        [TestMethod]
        public void MovingFrozenItemToWarmer_Thaws()
        {
            var item = AddTo(freezer);

            Assert.IsTrue(Move(item, freezer, warmer));
            Assert.AreEqual("thawed", item.GetStage());

            Assert.IsTrue(warmer.GetItems().Contains(item));
            Assert.IsFalse(freezer.GetItems().Contains(item));
        }

        [TestMethod]
        public void ThawedItem_CannotBeRefrozen()
        {
            var item = AddTo(freezer);

            Assert.IsTrue(Move(item, freezer, warmer));
            Assert.IsFalse(Move(item, warmer, freezer));

            Assert.AreEqual("thawed", item.GetStage());
        }

        [TestMethod]
        public void FailedMove_KeepsStageAndLocation()
        {
            for (int i = 0; i < 3; i++)
                AddTo(warmer, NewIceCream(3, "Filler" + i));

            var item = AddTo(freezer);

            Assert.IsFalse(Move(item, freezer, warmer));
            Assert.AreEqual("frozen", item.GetStage());
            Assert.IsTrue(freezer.GetItems().Contains(item));
        }

        [TestMethod]
        public void RemovingExpiredItem_IsThrown_WithTodaysDate()
        {
            var item = AddTo(warmer, NewIceCream(-1));

            warmer.RemoveItem(item);

            Assert.IsTrue(
                GlobalIDs.GetThrownThisMonthList().Contains(item));

            var diff = DateTime.Now - item.GetdateOfThrowing();

            Assert.IsTrue(
                diff >= TimeSpan.FromMinutes(-1) &&
                diff < TimeSpan.FromMinutes(2));
        }

        [TestMethod]
        public void RemovingFreshItem_IsNotThrown()
        {
            var item = AddTo(warmer, NewIceCream(3));

            warmer.RemoveItem(item);

            Assert.IsFalse(
                GlobalIDs.GetThrownThisMonthList().Contains(item));
        }

        [TestMethod]
        public void MovingExpiredItem_IsNotThrown()
        {
            var item = AddTo(freezer, NewIceCream(-1));

            Assert.IsTrue(Move(item, freezer, warmer));

            Assert.IsFalse(
                GlobalIDs.GetThrownThisMonthList().Contains(item));
        }
    }
}
