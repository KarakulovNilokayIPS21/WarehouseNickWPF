using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Text;
using WarehouseNickData.Context;
using WarehouseNickData.Models;

namespace WarehouseNickData.Services
{
    public interface IWarehouseService
    {
        IEnumerable<Warehouse> GetAll();
        Warehouse? GetById(int id);
        IEnumerable<Warehouse> GetByOrganization(int organizationId);
        void Add(Warehouse warehouse);
        void Update(Warehouse warehouse);
        void Delete(int id);
    }
}

namespace WarehouseNickData.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly ApplicationContext _context;

        public WarehouseService(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Warehouse> GetAll() => _context.Warehouses;

        public Warehouse? GetById(int id) => _context.Warehouses.FirstOrDefault(w => w.Id == id);

        public IEnumerable<Warehouse> GetByOrganization(int organizationId) =>
            _context.Warehouses.Where(w => w.OrganizationId == organizationId);

        public void Add(Warehouse warehouse)
        {
            warehouse.Id = _context.Warehouses.Any() ? _context.Warehouses.Max(w => w.Id) + 1 : 1;
            _context.Warehouses.Add(warehouse);
        }

        public void Update(Warehouse warehouse)
        {
            var existing = GetById(warehouse.Id);
            if (existing != null)
            {
                existing.Name = warehouse.Name;
                existing.OrganizationId = warehouse.OrganizationId;
            }
        }

        public void Delete(int id)
        {
            var warehouse = GetById(id);
            if (warehouse != null) _context.Warehouses.Remove(warehouse);
        }
    }
}