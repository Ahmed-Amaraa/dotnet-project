using System.Threading.Tasks;
using InventoryManagement.Web.Models;
using InventoryManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Web.Data;

namespace InventoryManagement.Web.Controllers;

public class SuppliersController : Controller
{
    private readonly ApplicationDbContext _context;

    public SuppliersController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var suppliers = _context.Suppliers.ToList();
        return View(suppliers);
    }

    public IActionResult Details(int id)
    {
        var supplier = _context.Suppliers.FirstOrDefault(s => s.SupplierId == id);
        if (supplier == null) return NotFound();
        return View(supplier);
    }

    public IActionResult Create()
    {
        return View(new Supplier());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Supplier supplier)
    {
        if (!ModelState.IsValid)
        {
            return View(supplier);
        }
        _context.Suppliers.Add(supplier);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var supplier = _context.Suppliers.Find(id);
        if (supplier == null) return NotFound();
        return View(supplier);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Supplier supplier)
    {
        if (!ModelState.IsValid)
        {
            return View(supplier);
        }
        _context.Suppliers.Update(supplier);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var supplier = _context.Suppliers.Find(id);
        if (supplier == null) return NotFound();
        return View(supplier);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var supplier = _context.Suppliers.Find(id);
        if (supplier != null)
        {
            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }
}
