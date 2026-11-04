using KodaStore.Api.DTOs;
using KodaStore.Api.Services;
using Microsoft.AspNetCore.Mvc;
using KodaStore.Api.ViewModels;
using System.ComponentModel;
namespace KodaStore.Api.Controllers;

[Route("catalog")]
public sealed class CatalogController : Controller
{
    private readonly IProductService _service;
    public CatalogController(IProductService service)
    {
        _service = service;
    }

    [HttpGet("")]
    public IActionResult Index() => View(_service.GetAll());

    [HttpGet("details/{productCode}")]
    public IActionResult Details(string productCode)
    {
        var product = _service.GetByCode(productCode);

        return product is null ? NotFound() : View(product);
    }

    [HttpGet("create")]
    public IActionResult Create() => View(new CreateProductViewModel());
    // {
    //     return View(new CreateProductViewModel());
    // }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateProductViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        try
        {
            var request = new ProductRequest(model.ProductCode, model.Name, model.Price!.Value, model.Stock!.Value, model.Category);

            var created = _service.Create(request);
            if (created is null)
            {
                ModelState.AddModelError(nameof(model.ProductCode), "ProductCode Already Exists");

                return View(model);
            }
            TempData["Success"] = "Product berhasil ditambahkan";

            return RedirectToAction(
                nameof(Details),
                new { productCode = created.ProductCode });
        }
        catch (ArgumentException e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return View(model);
        }
    }

    [HttpGet("edit/{productCode}")]
    public IActionResult Edit(string productCode)
    {
        var product = _service.GetByCode(productCode);

        if (product is null) return NotFound();

        ViewData["ProductCode"] = product.ProductCode;

        return View(new EditProductViewModel
        {
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
            Category = product.Category
        });
    }

    [HttpPost("edit/{productCode}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(string productCode, EditProductViewModel model)
    {
        ViewData["ProductCode"] = productCode;
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var request = new UpdateProductRequest(
                model.Name, model.Price!.Value,
                model.Stock!.Value, model.Category);
            var updated = _service.Update(productCode, request);
            if (updated is null) return NotFound();

            TempData["Success"] = "Produk berhasil diperbarui.";
            return RedirectToAction(nameof(Details),
                new { productCode = updated.ProductCode });
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet("delete/{productCode}")]
    public IActionResult Delete(string productCode)
    {
        var product = _service.GetByCode(productCode);

        return product is null ? NotFound() : View(product);
    }

    [HttpPost("delete/{productCode}")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(string productCode)
    {
        if (!_service.Delete(productCode)) return NotFound();

        TempData["Success"] = "Product Deleted";

        return RedirectToAction(nameof(Index));
    }
}