using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Text;
using WarehouseNickData.Context;
using WarehouseNickData.Models;

namespace WarehouseNickData.Services
{
    public interface IManufacturerService
    {
        IEnumerable<Manufacturer> GetAll();
        Manufacturer? GetById(int id);
        void Add(Manufacturer manufacturer);
        void Update(Manufacturer manufacturer);
        void Delete(int id);
        bool ExistsName(string name, int? excludeId = null);
    }
}
namespace WarehouseNickData.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly ApplicationContext _context;

        public ManufacturerService(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Manufacturer> GetAll() => _context.Manufacturers;

        public Manufacturer? GetById(int id) => _context.Manufacturers.FirstOrDefault(m => m.Id == id);

        public void Add(Manufacturer manufacturer)
        {
            manufacturer.Id = _context.Manufacturers.Any()
                ? _context.Manufacturers.Max(m => m.Id) + 1
                : 1;
            _context.Manufacturers.Add(manufacturer);
        }

        public void Update(Manufacturer manufacturer)
        {
            var existing = GetById(manufacturer.Id);
            if (existing != null)
                existing.Name = manufacturer.Name;
        }

        public void Delete(int id)
        {
            var manufacturer = GetById(id);
            if (manufacturer != null)
                _context.Manufacturers.Remove(manufacturer);
        }

        public bool ExistsName(string name, int? excludeId = null)
        {
            return _context.Manufacturers.Any(m =>
                m.Name == name && (!excludeId.HasValue || m.Id != excludeId));
        }
    }
}
