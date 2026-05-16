using System;
using System.Collections.Generic;
using System.Text;

using WarehouseNickData.Context;
using WarehouseNickData.Models;

namespace WarehouseNickData.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ApplicationContext _context;

        public OrganizationService(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Organization> GetAll() => _context.Organizations;

        public Organization? GetById(int id) => _context.Organizations.FirstOrDefault(o => o.Id == id);

        public void Add(Organization org)
        {
            org.Id = _context.Organizations.Any() ? _context.Organizations.Max(o => o.Id) + 1 : 1;
            _context.Organizations.Add(org);
        }

        public void Update(Organization org)
        {
            var existing = GetById(org.Id);
            if (existing != null) existing.Name = org.Name;
        }

        public void Delete(int id)
        {
            var org = GetById(id);
            if (org != null) _context.Organizations.Remove(org);
        }

        public bool ExistsName(string name, int? excludeId = null)
        {
            return _context.Organizations.Any(o => o.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && (!excludeId.HasValue || o.Id != excludeId.Value));
        }
    }
}