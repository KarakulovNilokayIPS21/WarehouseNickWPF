using WarehouseNickData.Context;

namespace WarehouseDataTest_MSTest.Helpers
{
    public static class TestHelper
    {
        public static ApplicationContext CreateFreshContext()
        {
            var context = new ApplicationContext();
            context.Organizations.Clear();
            context.Warehouses.Clear();
            context.Categories.Clear();
            context.Manufacturers.Clear();
            context.Suppliers.Clear();
            context.Products.Clear();
            return context;
        }
    }
}