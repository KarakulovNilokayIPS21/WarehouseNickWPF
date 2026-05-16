using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Text;
using WarehouseNickData.Context;
using WarehouseNickData.Models;

namespace WarehouseNickData.Services
{
    public interface ISupplierService
    {
        IEnumerable<Supplier> GetAll();
        Supplier? GetById(int id);
        void Add(Supplier supplier);
        void Update(Supplier supplier);
        void Delete(int id);
        bool ExistsName(string name, int? excludeId = null);
    }
}
namespace WarehouseNickData.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ApplicationContext _context;

        public SupplierService(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Supplier> GetAll() => _context.Suppliers;

        public Supplier? GetById(int id) => _context.Suppliers.FirstOrDefault(s => s.Id == id);

        public void Add(Supplier supplier)
        {
            supplier.Id = _context.Suppliers.Any()
                ? _context.Suppliers.Max(s => s.Id) + 1
                : 1;
            _context.Suppliers.Add(supplier);
        }

        public void Update(Supplier supplier)
        {
            var existing = GetById(supplier.Id);
            if (existing != null)
                existing.Name = supplier.Name;
        }

        public void Delete(int id)
        {
            var supplier = GetById(id);
            if (supplier != null)
                _context.Suppliers.Remove(supplier);
        }

        public bool ExistsName(string name, int? excludeId = null)
        {
            return _context.Suppliers.Any(s =>
                s.Name == name && (!excludeId.HasValue || s.Id != excludeId));
        }
    }
}