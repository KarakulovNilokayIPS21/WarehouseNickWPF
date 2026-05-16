using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WarehouseNickData.Models;
using WarehouseNickData.Services;
using WarehouseDataTest_MSTest.Helpers;

namespace WarehouseDataTest.Services
{
    [TestClass]
    public class OrganizationServiceTests
    {
        [TestMethod]
        public void Add_ShouldIncreaseCountAndSetId()
        {
            var context = TestHelper.CreateFreshContext();
            var service = new OrganizationService(context);
            var org = new Organization { Name = "Тест" };

            service.Add(org);

            Assert.AreEqual(1, service.GetAll().Count());
            Assert.AreEqual(1, org.Id);
        }

        [TestMethod]
        public void Delete_ShouldRemoveOrganization()
        {
            var context = TestHelper.CreateFreshContext();
            var service = new OrganizationService(context);
            var org = new Organization { Name = "Удалить" };
            service.Add(org);
            int id = org.Id;

            service.Delete(id);

            Assert.IsNull(service.GetById(id));
        }

        [TestMethod]
        public void Update_ShouldChangeName()
        {
            var context = TestHelper.CreateFreshContext();
            var service = new OrganizationService(context);
            var org = new Organization { Name = "Старое" };
            service.Add(org);
            org.Name = "Новое";

            service.Update(org);
            var updated = service.GetById(org.Id);

            Assert.AreEqual("Новое", updated.Name);
        }

        [TestMethod]
        public void ExistsName_ReturnsTrueForDuplicate()
        {
            var context = TestHelper.CreateFreshContext();
            var service = new OrganizationService(context);
            service.Add(new Organization { Name = "Дубль" });

            Assert.IsTrue(service.ExistsName("Дубль"));
        }

        [TestMethod]
        public void ExistsName_ReturnsFalseForUnique()
        {
            var context = TestHelper.CreateFreshContext();
            var service = new OrganizationService(context);
            service.Add(new Organization { Name = "Одна" });

            Assert.IsFalse(service.ExistsName("Другая"));
        }
    }
}