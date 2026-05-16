using WarehouseNickData.Models;

namespace WarehouseNickData.Context
{
    public class ApplicationContext
    {
        public List<Organization> Organizations { get; set; } = new();
        public List<Warehouse> Warehouses { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<Manufacturer> Manufacturers { get; set; } = new();
        public List<Supplier> Suppliers { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public List<Invoice> Invoices { get; set; } = new();
        public List<InvoiceItem> InvoiceItems { get; set; } = new();
        public List<WarehouseProduct> WarehouseProducts { get; set; } = new();



        public void SeedData()
        {
            if (!Organizations.Any())
            {
                Organizations.AddRange(new[]
                {
            new Organization { Id = 1, Name = "Рога и копыта, ООО" },
            new Organization { Id = 2, Name = "Пупкин и сыновья, ООО" }
        });
            }

            if (!Warehouses.Any())
            {
                Warehouses.AddRange(new[]
                {
            new Warehouse { Id = 1, Name = "Склад Рогов и Копыт №1", OrganizationId = 1 },
            new Warehouse { Id = 2, Name = "Склад Рогов и Копыт №2", OrganizationId = 1 },
            new Warehouse { Id = 3, Name = "Основной склад Пупкиных", OrganizationId = 2 }
        });
            }

            if (!Categories.Any())
            {
                Categories.AddRange(new[]
                {
            new Category { Id = 1, Name = "Рога" },
            new Category { Id = 2, Name = "Копыта" },
            new Category { Id = 3, Name = "Аксессуары" }
        });
            }

            if (!Manufacturers.Any())
            {
                Manufacturers.AddRange(new[]
                {
            new Manufacturer { Id = 1, Name = "РогИзделие" },
            new Manufacturer { Id = 2, Name = "КопытПром" }
        });
            }

            if (!Suppliers.Any())
            {
                Suppliers.AddRange(new[]
                {
            new Supplier { Id = 1, Name = "ООО РогаОпт" },
            new Supplier { Id = 2, Name = "ИП Копытов" }
        });
            }

            if (!Products.Any())
            {
                Products.AddRange(new[]
                {
        new Product { Id = 1, Name = "Рог оленя", CategoryId = 1, ManufacturerId = 1, SupplierId = 1, Price = 1500, Stock = 10, Discount = 0 },
        new Product { Id = 2, Name = "Копыто коровы", CategoryId = 2, ManufacturerId = 2, SupplierId = 2, Price = 800, Stock = 25, Discount = 5 },
        // ДОБАВЬТЕ НОВЫЕ ТОВАРЫ:
        new Product { Id = 3, Name = "Копыто лося", CategoryId = 2, ManufacturerId = 2, SupplierId = 2, Price = 1200, Stock = 7, Discount = 0 },
        new Product { Id = 4, Name = "Рог барана", CategoryId = 1, ManufacturerId = 1, SupplierId = 1, Price = 2000, Stock = 3, Discount = 10 },
        new Product { Id = 5, Name = "Подкова", CategoryId = 3, ManufacturerId = 1, SupplierId = 2, Price = 500, Stock = 20, Discount = 0 }
    });
                if (!WarehouseProducts.Any())
                {
                    WarehouseProducts.AddRange(new[]
                    {
            new WarehouseProduct { Id = 1, WarehouseId = 1, ProductId = 1, Stock = 10, Price = 1500, Discount = 0 },
            new WarehouseProduct { Id = 2, WarehouseId = 1, ProductId = 2, Stock = 5,  Price = 800,  Discount = 5 },
            new WarehouseProduct { Id = 3, WarehouseId = 2, ProductId = 1, Stock = 0,  Price = 1600, Discount = 10 },
            new WarehouseProduct { Id = 4, WarehouseId = 2, ProductId = 2, Stock = 15, Price = 750,  Discount = 0 },
            new WarehouseProduct { Id = 5, WarehouseId = 3, ProductId = 2, Stock = 8,  Price = 900,  Discount = 15 },
             new WarehouseProduct { Id = 6, WarehouseId = 1, ProductId = 3, Stock = 3, Price = 1200, Discount = 0 },
        new WarehouseProduct { Id = 7, WarehouseId = 1, ProductId = 4, Stock = 1, Price = 2000, Discount = 10 },
        new WarehouseProduct { Id = 8, WarehouseId = 1, ProductId = 5, Stock = 10, Price = 500, Discount = 0 },
        });
                }
            }
        }
    }
}