using AzureBlobTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobTest.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{

    private readonly AzureBlobServices _azureBlobServices;

    public FileController(AzureBlobServices azureBlobServices){
        _azureBlobServices = azureBlobServices;
    }
    
    [HttpGet]
    public async Task<IActionResult> ListBlobs(){
        Console.WriteLine("---------list blobs from file controller-----------");
        var result = await _azureBlobServices.ListAsync();
        // return Ok(result);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file){
        Console.WriteLine("---------Upload from file comtroller-----------");
        
        var result = await _azureBlobServices.UploadAsync(file);
        return Ok(result);

    }
    
    [HttpGet]
    [Route("filename")]
    public async Task<IActionResult> Download(string filename){
        Console.WriteLine("---------Download from file controller-----------");
        var result = await _azureBlobServices.DownloadAsync(filename);
        return File(result.Content, result.ContentType, result.Name);
    }
    // [HttpDelete]
    [HttpGet]
    [Route("filename1")]
    public async Task<IActionResult> Delete(string filename1){
        Console.WriteLine("---------Delete from file controller-----------");
        var result = await _azureBlobServices.DeleteAsync(filename1);
        return Ok(result);
    }

}