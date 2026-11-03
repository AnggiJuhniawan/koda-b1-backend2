using KodaStore.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace KodaStore.Api.Controllers;

public sealed class CatalogController : Controller
{
    private readonly IProductService _service;
    public CatalogController(IProductService service)
    {
        _service = service;
    }

    [HttpGet("/catalog")]
    public IActionResult Index()
    {
        var products = _service.GetAll();
        return View(products);
    }
}