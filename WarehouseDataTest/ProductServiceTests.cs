using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarehouseDataTest_MSTest.Helpers;
using WarehouseNickData.Context;
using WarehouseNickData.Models;
using WarehouseNickData.Services;

namespace WarehouseDataTest.Services
{
    [TestClass]
    public class ProductServiceTests
    {
        private void AddDependencies(ApplicationContext ctx)
        {
            ctx.Categories.Add(new Category { Id = 1, Name = "Категория" });
            ctx.Manufacturers.Add(new Manufacturer { Id = 1, Name = "Производитель" });
            ctx.Suppliers.Add(new Supplier { Id = 1, Name = "Поставщик" });
        }



        [TestMethod]
        public void GetByWarehouse_ReturnsEmptyIfNoProducts()
        {
            var ctx = TestHelper.CreateFreshContext();
            var service = new ProductService(ctx);
            var result = service.GetByWarehouse(99);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetById_ReturnsCorrectProduct()
        {
            var ctx = TestHelper.CreateFreshContext();
            AddDependencies(ctx);
            var service = new ProductService(ctx);
            var product = new Product { Name = "Найди меня", CategoryId = 1, ManufacturerId = 1, SupplierId = 1 };
            service.Add(product, 1);

            var found = service.GetById(product.Id);
            Assert.IsNotNull(found);
            Assert.AreEqual("Найди меня", found.Name);
        }

        [TestMethod]
        public void Update_ChangesProductProperties()
        {
            var ctx = TestHelper.CreateFreshContext();
            AddDependencies(ctx);
            var service = new ProductService(ctx);
            var product = new Product { Name = "Старое", CategoryId = 1, ManufacturerId = 1, SupplierId = 1, Price = 10, Stock = 1 };
            service.Add(product, 1);

            product.Name = "Новое";
            product.Price = 20;
            service.Update(product);

            var updated = service.GetById(product.Id);
            Assert.AreEqual("Новое", updated.Name);
            Assert.AreEqual(20, updated.Price);
        }


        [TestMethod]
        public void GetCategoryName_ReturnsCorrectString()
        {
            var ctx = TestHelper.CreateFreshContext();
            ctx.Categories.Add(new Category { Id = 5, Name = "Электроника" });
            var service = new ProductService(ctx);
            var name = service.GetCategoryName(5);
            Assert.AreEqual("Электроника", name);
        }

        [TestMethod]
        public void GetCategoryName_ReturnsEmptyForMissingId()
        {
            var ctx = TestHelper.CreateFreshContext();
            var service = new ProductService(ctx);
            var name = service.GetCategoryName(999);
            Assert.AreEqual(string.Empty, name);
        }
    }
}