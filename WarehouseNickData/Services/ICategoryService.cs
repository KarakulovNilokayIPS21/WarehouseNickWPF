using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Text;
using WarehouseNickData.Context;
using WarehouseNickData.Models;

namespace WarehouseNickData.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();
        Category? GetById(int id);
        void Add(Category category);
        void Update(Category category);
        void Delete(int id);
        bool ExistsName(string name, int? excludeId = null);
    }
}
namespace WarehouseNickData.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationContext _context;

        public CategoryService(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAll() => _context.Categories ?? new List<Category>();

        public Category? GetById(int id) => _context.Categories.FirstOrDefault(c => c.Id == id);

        public void Add(Category category)
        {
            category.Id = _context.Categories.Any() ? _context.Categories.Max(c => c.Id) + 1 : 1;
            _context.Categories.Add(category);
        }

        public void Update(Category category)
        {
            var existing = GetById(category.Id);
            if (existing != null) existing.Name = category.Name;
        }

        public void Delete(int id)
        {
            var category = GetById(id);
            if (category != null) _context.Categories.Remove(category);
        }

        public bool ExistsName(string name, int? excludeId = null)
        {
            return _context.Categories.Any(c => c.Name == name && (!excludeId.HasValue || c.Id != excludeId));
        }
    }
}