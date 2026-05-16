using System;
using System.Collections.Generic;
using System.Text;

using WarehouseNickData.Models;

namespace WarehouseNickData.Services
{
    public interface IOrganizationService
    {
        IEnumerable<Organization> GetAll();
        Organization? GetById(int id);
        void Add(Organization org);
        void Update(Organization org);
        void Delete(int id);
        bool ExistsName(string name, int? excludeId = null);
    }
}