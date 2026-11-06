
using KodaStore.Api.DTOs;
using KodaStore.Api.Services;
using KodaStore.Api.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KodaStore.Api.Controllers;

[Route("catalog")]
public sealed class CatalogController : Controller
{
    private readonly IProductService _service;

    // Constructor Injection
    public CatalogController(IProductService service)
    {
        _service = service;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        var products = await _service.GetAllAsync(
            cancellationToken);

        return View(products);
    }

    [HttpGet("details/{productCode}")]
    public async Task<IActionResult> Details(
        string productCode,
        CancellationToken cancellationToken)
    {
        var product = await _service.GetByCodeAsync(
            productCode,
            cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new CreateProductViewModel());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateProductViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // Mapping ViewModel -> DTO
            var request = new ProductRequest(
    model.ProductCode,
    model.Name,
    model.Price ?? 0, // Jika null, anggap saja 0 (meski tidak mungkin null karena ModelState)
    model.Stock ?? 0, // Jika null, anggap saja 0
    model.Category
);

            // Service -> Repository -> SQL Server
            var created = await _service.CreateAsync(
                request,
                cancellationToken);

            // ProductCode already exists
            if (created is null)
            {
                ModelState.AddModelError(
                    nameof(model.ProductCode),
                    "Kode produk sudah digunakan.");

                return View(model);
            }

            // Success message
            TempData["Success"] =
                "Produk berhasil ditambahkan.";

            // Redirect after successful POST
            return RedirectToAction(
                nameof(Details),
                new { productCode = created.ProductCode });
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(model);
        }
    }

    [HttpGet("edit/{productCode}")]
    public async Task<IActionResult> Edit(
        string productCode,
        CancellationToken cancellationToken)
    {
        var product = await _service.GetByCodeAsync(
            productCode,
            cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        ViewData["ProductCode"] = product.ProductCode;

        // Mapping Response DTO -> ViewModel
        var model = new EditProductViewModel
        {
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
            Category = product.Category
        };

        return View(model);
    }

    [HttpPost("edit/{productCode}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        string productCode,
        EditProductViewModel model,
        CancellationToken cancellationToken)
    {
        ViewData["ProductCode"] = productCode;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // Mapping ViewModel -> DTO
            var request = new UpdateProductRequest
            (
                model.Name,
                model.Price ?? 0, // Jika null, anggap saja 0 (meski tidak mungkin null karena ModelState)
                model.Stock ?? 0, // Jika null, anggap saja 0
                model.Category
            );

            // Service -> Repository -> SQL UPDATE
            var updated = await _service.UpdateAsync(
                productCode,
                request,
                cancellationToken);

            if (updated is null)
            {
                return NotFound();
            }

            TempData["Success"] =
                "Produk berhasil diperbarui.";

            return RedirectToAction(
                nameof(Details),
                new { productCode = updated.ProductCode });
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(
                string.Empty,
                ex.Message);

            return View(model);
        }
    }

    [HttpGet("delete/{productCode}")]
    public async Task<IActionResult> Delete(
        string productCode,
        CancellationToken cancellationToken)
    {
        var product = await _service.GetByCodeAsync(
            productCode,
            cancellationToken);

        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost("delete/{productCode}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(
        string productCode,
        CancellationToken cancellationToken)
    {
        // Service -> Repository -> SQL DELETE
        var deleted = await _service.DeleteAsync(
            productCode,
            cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        TempData["Success"] =
            "Produk berhasil dihapus.";

        return RedirectToAction(nameof(Index));
    }
}

// using KodaStore.Api.DTOs;
// using KodaStore.Api.Services;
// using Microsoft.AspNetCore.Mvc;
// using KodaStore.Api.ViewModels;
// using System.ComponentModel;
// namespace KodaStore.Api.Controllers;

// [Route("catalog")]
// public sealed class CatalogController : Controller
// {
//     private readonly IProductService _service;
//     public CatalogController(IProductService service)
//     {
//         _service = service;
//     }

//     [HttpGet("")]
//     public IActionResult Index() => View(_service.GetAll());

//     [HttpGet("details/{productCode}")]
//     public IActionResult Details(string productCode)
//     {
//         var product = _service.GetByCode(productCode);

//         return product is null ? NotFound() : View(product);
//     }

//     [HttpGet("create")]
//     public IActionResult Create() => View(new CreateProductViewModel());
//     // {
//     //     return View(new CreateProductViewModel());
//     // }

//     [HttpPost("create")]
//     [ValidateAntiForgeryToken]
//     public IActionResult Create(CreateProductViewModel model)
//     {
//         if (!ModelState.IsValid) return View(model);
//         try
//         {
//             var request = new ProductRequest(model.ProductCode, model.Name, model.Price!.Value, model.Stock!.Value, model.Category);

//             var created = _service.Create(request);
//             if (created is null)
//             {
//                 ModelState.AddModelError(nameof(model.ProductCode), "ProductCode Already Exists");

//                 return View(model);
//             }
//             TempData["Success"] = "Product berhasil ditambahkan";

//             return RedirectToAction(
//                 nameof(Details),
//                 new { productCode = created.ProductCode });
//         }
//         catch (ArgumentException e)
//         {
//             ModelState.AddModelError(string.Empty, e.Message);
//             return View(model);
//         }
//     }

//     [HttpGet("edit/{productCode}")]
//     public IActionResult Edit(string productCode)
//     {
//         var product = _service.GetByCode(productCode);

//         if (product is null) return NotFound();

//         ViewData["ProductCode"] = product.ProductCode;

//         return View(new EditProductViewModel
//         {
//             Name = product.Name,
//             Price = product.Price,
//             Stock = product.Stock,
//             Category = product.Category
//         });
//     }

//     [HttpPost("edit/{productCode}")]
//     [ValidateAntiForgeryToken]
//     public IActionResult Edit(string productCode, EditProductViewModel model)
//     {
//         ViewData["ProductCode"] = productCode;
//         if (!ModelState.IsValid)
//             return View(model);

//         try
//         {
//             var request = new UpdateProductRequest(
//                 model.Name, model.Price!.Value,
//                 model.Stock!.Value, model.Category);
//             var updated = _service.Update(productCode, request);
//             if (updated is null) return NotFound();

//             TempData["Success"] = "Produk berhasil diperbarui.";
//             return RedirectToAction(nameof(Details),
//                 new { productCode = updated.ProductCode });
//         }
//         catch (ArgumentException ex)
//         {
//             ModelState.AddModelError(string.Empty, ex.Message);
//             return View(model);
//         }
//     }

//     [HttpGet("delete/{productCode}")]
//     public IActionResult Delete(string productCode)
//     {
//         var product = _service.GetByCode(productCode);

//         return product is null ? NotFound() : View(product);
//     }

//     [HttpPost("delete/{productCode}")]
//     [ValidateAntiForgeryToken]
//     public IActionResult DeleteConfirmed(string productCode)
//     {
//         if (!_service.Delete(productCode)) return NotFound();

//         TempData["Success"] = "Product Deleted";

//         return RedirectToAction(nameof(Index));
//     }
// }