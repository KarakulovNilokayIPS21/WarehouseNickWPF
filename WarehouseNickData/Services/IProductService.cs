using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Text;
using WarehouseNickData.Context;
using WarehouseNickData.Models;

namespace WarehouseNickData.Services
{



    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        Product? GetById(int id);
        IEnumerable<Product> GetByWarehouse(int warehouseId); 
        void Add(Product product, int warehouseId);
        void Update(Product product);
        void Delete(int id);

        string GetCategoryName(int categoryId);
        string GetManufacturerName(int manufacturerId);
        string GetSupplierName(int supplierId);
    }
}




namespace WarehouseNickData.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationContext _context;
        private readonly Dictionary<int, List<int>> _warehouseProducts; 

        public ProductService(ApplicationContext context)
        {
            _context = context;
            _warehouseProducts = new Dictionary<int, List<int>>();
        }

        public IEnumerable<Product> GetAll() => _context.Products;

        public Product? GetById(int id) => _context.Products.FirstOrDefault(p => p.Id == id);

        public IEnumerable<Product> GetByWarehouse(int warehouseId)
        {
            var productIds = _context.WarehouseProducts
                .Where(wp => wp.WarehouseId == warehouseId)
                .Select(wp => wp.ProductId);
            return _context.Products.Where(p => productIds.Contains(p.Id));
        }

        public void Add(Product product, int warehouseId)
        {
            product.Id = _context.Products.Any() ? _context.Products.Max(p => p.Id) + 1 : 1;
            _context.Products.Add(product);

            if (!_warehouseProducts.ContainsKey(warehouseId))
                _warehouseProducts[warehouseId] = new List<int>();
            _warehouseProducts[warehouseId].Add(product.Id);
        }

        public void Update(Product product)
        {
            var existing = GetById(product.Id);
            if (existing != null)
            {
                existing.Name = product.Name;
                existing.CategoryId = product.CategoryId;
                existing.ManufacturerId = product.ManufacturerId;
                existing.SupplierId = product.SupplierId;
                existing.Price = product.Price;
                existing.Stock = product.Stock;
                existing.Discount = product.Discount;
            }
        }


        public void Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                foreach (var kv in _warehouseProducts)
                    kv.Value.Remove(id);
            }
        }

        public string GetCategoryName(int categoryId)
        {
            var cat = _context.Categories.FirstOrDefault(c => c.Id == categoryId);
            return cat?.Name ?? string.Empty;
        }

        public string GetManufacturerName(int manufacturerId)
        {
            var man = _context.Manufacturers.FirstOrDefault(m => m.Id == manufacturerId);
            return man?.Name ?? string.Empty;
        }

        public string GetSupplierName(int supplierId)
        {
            var sup = _context.Suppliers.FirstOrDefault(s => s.Id == supplierId);
            return sup?.Name ?? string.Empty;
        }
    }
}
