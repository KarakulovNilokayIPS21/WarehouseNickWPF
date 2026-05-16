using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarehouseDataTest_MSTest.Helpers;
using WarehouseNickData.Models;
using WarehouseNickData.Services;

namespace WarehouseDataTest.Services
{
    [TestClass]
    public class WarehouseServiceTests
    {
        [TestMethod]
        public void Add_ShouldIncreaseCountAndSetId()
        {
            var ctx = TestHelper.CreateFreshContext();
            var service = new WarehouseService(ctx);
            var wh = new Warehouse { Name = "Новый склад", OrganizationId = 1 };

            service.Add(wh);

            Assert.AreEqual(1, service.GetAll().Count());
            Assert.AreEqual(1, wh.Id);
        }

        [TestMethod]
        public void GetByOrganization_ReturnsWarehousesOnlyForGivenOrg()
        {
            var ctx = TestHelper.CreateFreshContext();
            ctx.Organizations.Add(new Organization { Id = 1, Name = "O1" });
            ctx.Organizations.Add(new Organization { Id = 2, Name = "O2" });
            ctx.Warehouses.Add(new Warehouse { Id = 1, Name = "Склад1", OrganizationId = 1 });
            ctx.Warehouses.Add(new Warehouse { Id = 2, Name = "Склад2", OrganizationId = 1 });
            ctx.Warehouses.Add(new Warehouse { Id = 3, Name = "Склад3", OrganizationId = 2 });
            var service = new WarehouseService(ctx);

            var forOrg1 = service.GetByOrganization(1);
            var forOrg2 = service.GetByOrganization(2);

            Assert.AreEqual(2, forOrg1.Count());
            Assert.AreEqual(1, forOrg2.Count());
            Assert.IsTrue(forOrg1.Any(w => w.Name == "Склад1"));
            Assert.IsTrue(forOrg2.Any(w => w.Name == "Склад3"));
        }

        [TestMethod]
        public void Update_ShouldChangeName()
        {
            var ctx = TestHelper.CreateFreshContext();
            var service = new WarehouseService(ctx);
            var wh = new Warehouse { Name = "Старое имя", OrganizationId = 1 };
            service.Add(wh);
            wh.Name = "Новое имя";

            service.Update(wh);
            var updated = service.GetById(wh.Id);

            Assert.AreEqual("Новое имя", updated.Name);
        }

        [TestMethod]
        public void Delete_ShouldRemoveWarehouse()
        {
            var ctx = TestHelper.CreateFreshContext();
            var service = new WarehouseService(ctx);
            var wh = new Warehouse { Name = "Удалить", OrganizationId = 1 };
            service.Add(wh);
            int id = wh.Id;

            service.Delete(id);

            Assert.IsNull(service.GetById(id));
            Assert.AreEqual(0, service.GetAll().Count());
        }
    }
}