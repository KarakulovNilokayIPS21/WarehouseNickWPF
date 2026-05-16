using Microsoft.VisualStudio.TestTools.UnitTesting;
using WarehouseDataTest_MSTest.Helpers;
using WarehouseNickData.Models;
using WarehouseNickData.Services;

namespace WarehouseDataTest.Services
{
    [TestClass]
    public class CategoryServiceTests
    {
        [TestMethod]
        public void Add_ShouldSetIdAndIncreaseCount()
        {
            var ctx = TestHelper.CreateFreshContext();
            var service = new CategoryService(ctx);
            var cat = new Category { Name = "Новая" };

            service.Add(cat);
            Assert.AreEqual(1, cat.Id);
            Assert.AreEqual(1, service.GetAll().Count());
        }

        [TestMethod]
        public void Update_ShouldChangeName()
        {
            var ctx = TestHelper.CreateFreshContext();
            var service = new CategoryService(ctx);
            var cat = new Category { Name = "Старая" };
            service.Add(cat);
            cat.Name = "Новая";
            service.Update(cat);
            var updated = service.GetById(cat.Id);
            Assert.AreEqual("Новая", updated.Name);
        }

        [TestMethod]
        public void Delete_ShouldRemoveCategory()
        {
            var ctx = TestHelper.CreateFreshContext();
            var service = new CategoryService(ctx);
            var cat = new Category { Name = "Удалить" };
            service.Add(cat);
            int id = cat.Id;
            service.Delete(id);
            Assert.IsNull(service.GetById(id));
        }

        [TestMethod]
        public void ExistsName_ShouldReturnTrueForDuplicate()
        {
            var ctx = TestHelper.CreateFreshContext();
            var service = new CategoryService(ctx);
            service.Add(new Category { Name = "Дубль" });
            Assert.IsTrue(service.ExistsName("Дубль"));
        }
    }
}