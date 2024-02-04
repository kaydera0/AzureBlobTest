using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AzureBlobTest.Models;
using AzureBlobTest.Services;

namespace AzureBlobTest.Controllers;

// [ApiController]
// [Route("[controller]")]
public class HomeController : Controller
{
    private readonly AzureBlobServices _azureBlobServices;

    public HomeController(AzureBlobServices azureBlobServices){
        _azureBlobServices = azureBlobServices;
    }
    
    // private readonly ILogger<HomeController> _logger;
    //
    //
    // public HomeController(ILogger<HomeController> logger){
    //     _logger = logger;
    // }

    public IActionResult Index(){
        return View();
    }

    public IActionResult Privacy(){
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error(){
        return View(new ErrorViewModel{ RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    

}