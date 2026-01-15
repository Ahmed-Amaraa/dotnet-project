using System;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Web.Models;
using InventoryManagement.Web.Services;
using InventoryManagement.Web.ViewModels;
using InventoryManagement.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Web.Controllers;

public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var products = _context.Products.Include(p => p.Supplier).ToList();
        var suppliers = _context.Suppliers.ToList();
        var viewModel = new ProductListViewModel
        {
            Products = products,
            Suppliers = suppliers
        };
        return View(viewModel);
    }

    public IActionResult Details(int id)
    {
        var product = _context.Products.Include(p => p.Supplier).FirstOrDefault(p => p.ProductId == id);
        if (product == null) return NotFound();
        return View(product);
    }

    public IActionResult Create()
    {
        var suppliers = _context.Suppliers.ToList();
        var supplierSelectList = suppliers.Select(s => new SelectListItem
        {
            Value = s.SupplierId.ToString(),
            Text = s.SupplierName
        }).ToList();

        var viewModel = new ProductFormViewModel
        {
            Product = new Product(),
            Suppliers = supplierSelectList
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ProductFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var suppliers = _context.Suppliers.ToList();
            model.Suppliers = suppliers.Select(s => new SelectListItem
            {
                Value = s.SupplierId.ToString(),
                Text = s.SupplierName
            }).ToList();
            return View(model);
        }
        _context.Products.Add(model.Product);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null) return NotFound();
        var suppliers = _context.Suppliers.ToList();
        var supplierSelectList = suppliers.Select(s => new SelectListItem
        {
            Value = s.SupplierId.ToString(),
            Text = s.SupplierName
        }).ToList();
        var viewModel = new ProductFormViewModel
        {
            Product = product,
            Suppliers = supplierSelectList
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(ProductFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var suppliers = _context.Suppliers.ToList();
            model.Suppliers = suppliers.Select(s => new SelectListItem
            {
                Value = s.SupplierId.ToString(),
                Text = s.SupplierName
            }).ToList();
            return View(model);
        }
        _context.Products.Update(model.Product);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var product = _context.Products.Find(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }
}
