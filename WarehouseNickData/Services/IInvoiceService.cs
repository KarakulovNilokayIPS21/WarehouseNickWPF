using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseNickData.Context;
using WarehouseNickData.Models;

namespace WarehouseNickData.Services
{
    public interface IInvoiceService
    {
        IEnumerable<Invoice> GetAll();
        Invoice? GetById(int id);
        void Add(Invoice invoice);
        void Update(Invoice invoice);
        void Delete(int id);
        void ConfirmInvoice(int invoiceId);
        bool CanConfirm(Invoice invoice);
    }

    public class InvoiceService : IInvoiceService
    {
        private readonly ApplicationContext _context;
        private readonly IProductService _productService;

        public InvoiceService(ApplicationContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public IEnumerable<Invoice> GetAll() => _context.Invoices;

        public Invoice? GetById(int id) => _context.Invoices.FirstOrDefault(i => i.Id == id);

        public void Add(Invoice invoice)
        {
            invoice.Id = _context.Invoices.Any() ? _context.Invoices.Max(i => i.Id) + 1 : 1;
            invoice.Status = InvoiceStatus.Draft;
            foreach (var item in invoice.Items)
                item.Id = _context.InvoiceItems.Any() ? _context.InvoiceItems.Max(ii => ii.Id) + 1 : 1;
            _context.Invoices.Add(invoice);
            _context.InvoiceItems.AddRange(invoice.Items);
        }

        public void Update(Invoice invoice)
        {
            var existing = GetById(invoice.Id);
            if (existing == null || existing.Status == InvoiceStatus.Confirmed)
                throw new InvalidOperationException("Нельзя редактировать подтверждённую накладную");
            existing.Number = invoice.Number;
            existing.Date = invoice.Date;
            existing.Type = invoice.Type;
            existing.WarehouseId = invoice.WarehouseId;
            existing.Items.Clear();
            existing.Items.AddRange(invoice.Items);
        }

        public void Delete(int id)
        {
            var invoice = GetById(id);
            if (invoice != null && invoice.Status != InvoiceStatus.Confirmed)
            {
                _context.Invoices.Remove(invoice);
                foreach (var item in invoice.Items)
                    _context.InvoiceItems.Remove(item);
            }
        }

        public void ConfirmInvoice(int invoiceId)
        {
            var invoice = GetById(invoiceId);
            if (invoice == null || invoice.Status == InvoiceStatus.Confirmed)
                throw new InvalidOperationException("Накладная уже подтверждена или не существует");
            if (!CanConfirm(invoice))
                throw new InvalidOperationException("Накладная не может быть подтверждена (недостаточно товаров)");

            foreach (var item in invoice.Items)
            {
                var product = _productService.GetById(item.ProductId);
                if (product == null) continue;
                if (invoice.Type == InvoiceType.Incoming)
                    product.Stock += item.Quantity;
                else if (invoice.Type == InvoiceType.Outgoing)
                    product.Stock -= item.Quantity;
                _productService.Update(product);
            }
            invoice.Status = InvoiceStatus.Confirmed;
        }

        public bool CanConfirm(Invoice invoice)
        {
            if (invoice.Status != InvoiceStatus.Draft) return false;
            foreach (var item in invoice.Items)
            {
                var product = _productService.GetById(item.ProductId);
                if (product == null) return false;
                if (invoice.Type == InvoiceType.Outgoing && product.Stock < item.Quantity)
                    return false;
            }
            return true;
        }
    }
}