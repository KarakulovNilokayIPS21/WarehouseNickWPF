using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WarehouseDataTest_MSTest.Helpers;
using WarehouseNickData.Context;
using WarehouseNickData.Models;
using WarehouseNickData.Services;

namespace WarehouseDataTest.Services
{
    [TestClass]
    public class InvoiceServiceTests
    {
        private ApplicationContext CreateContextWithProduct()
        {
            var ctx = TestHelper.CreateFreshContext();
            ctx.Categories.Add(new Category { Id = 1, Name = "Кат" });
            ctx.Manufacturers.Add(new Manufacturer { Id = 1, Name = "Произв" });
            ctx.Suppliers.Add(new Supplier { Id = 1, Name = "Постав" });
            var product = new Product { Id = 1, Name = "Товар", CategoryId = 1, ManufacturerId = 1, SupplierId = 1, Price = 100, Stock = 10 };
            ctx.Products.Add(product);
            return ctx;
        }

        [TestMethod]
        public void ConfirmInvoice_Incoming_IncreasesStock()
        {
            var ctx = CreateContextWithProduct();
            var productService = new ProductService(ctx);
            var invoiceService = new InvoiceService(ctx, productService);

            var invoice = new Invoice
            {
                Number = "IN-001",
                Date = DateTime.Now,
                Type = InvoiceType.Incoming,
                WarehouseId = 1,
                Status = InvoiceStatus.Draft,
                Items = new List<InvoiceItem> { new InvoiceItem { ProductId = 1, Quantity = 5, PriceAtTime = 100 } }
            };
            invoiceService.Add(invoice);
            invoiceService.ConfirmInvoice(invoice.Id);

            var product = productService.GetById(1);
            Assert.AreEqual(15, product.Stock);
            Assert.AreEqual(InvoiceStatus.Confirmed, invoice.Status);
        }

        [TestMethod]
        public void ConfirmInvoice_Outgoing_SubtractsStock_IfEnough()
        {
            var ctx = CreateContextWithProduct();
            var productService = new ProductService(ctx);
            var invoiceService = new InvoiceService(ctx, productService);

            var invoice = new Invoice
            {
                Number = "OUT-001",
                Date = DateTime.Now,
                Type = InvoiceType.Outgoing,
                WarehouseId = 1,
                Status = InvoiceStatus.Draft,
                Items = new List<InvoiceItem> { new InvoiceItem { ProductId = 1, Quantity = 3, PriceAtTime = 100 } }
            };
            invoiceService.Add(invoice);
            invoiceService.ConfirmInvoice(invoice.Id);

            var product = productService.GetById(1);
            Assert.AreEqual(7, product.Stock);
        }



        [TestMethod]
        public void CanConfirm_ReturnsFalseForConfirmedInvoice()
        {
            var ctx = CreateContextWithProduct();
            var productService = new ProductService(ctx);
            var invoiceService = new InvoiceService(ctx, productService);
            var invoice = new Invoice
            {
                Number = "INV",
                Date = DateTime.Now,
                Type = InvoiceType.Incoming,
                WarehouseId = 1,
                Status = InvoiceStatus.Confirmed
            };
            Assert.IsFalse(invoiceService.CanConfirm(invoice));
        }

        [TestMethod]
        public void Add_ShouldGenerateIdAndSetDraftStatus()
        {
            var ctx = CreateContextWithProduct();
            var productService = new ProductService(ctx);
            var invoiceService = new InvoiceService(ctx, productService);
            var invoice = new Invoice
            {
                Number = "NEW",
                Date = DateTime.Now,
                Type = InvoiceType.Incoming,
                WarehouseId = 1,
                Items = new List<InvoiceItem> { new InvoiceItem { ProductId = 1, Quantity = 1 } }
            };
            invoiceService.Add(invoice);
            Assert.AreEqual(1, invoice.Id);
            Assert.AreEqual(InvoiceStatus.Draft, invoice.Status);
            Assert.AreEqual(1, invoiceService.GetAll().Count());
        }


    }
}
