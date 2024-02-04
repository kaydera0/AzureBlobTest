using Azure.Storage;
using Azure.Storage.Blobs;
using AzureBlobTest.Models;

namespace AzureBlobTest.Services;

public class AzureBlobServices
{

    private readonly string _storageName = "blobest";
    private readonly string _storageKey =
        "5+RhbNwSy5h9Hv1TSxQf26aaVW6bNuZduyp/4YcJ9ASU70wdJL1CkO+mZDtP6pI2XN+9KX0so4Yf+AStXGPT7A==";
    private readonly BlobContainerClient _filesContainer;

    public AzureBlobServices(){
        var credential = new StorageSharedKeyCredential(_storageName, _storageKey);
        var blobUri = $"https://{_storageName}.blob.core.windows.net";
        var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
        _filesContainer = blobServiceClient.GetBlobContainerClient("files");
    }

    public async Task<List<BlobDto>> ListAsync(){
        List<BlobDto> files = new List<BlobDto>();

        await foreach (var file in _filesContainer.GetBlobsAsync())
        {
            string uri = _filesContainer.Uri.ToString();
            var name = file.Name;
            var fullUri = $"{uri}/{name}";

            files.Add(new BlobDto{
                Uri = fullUri,
                Name = name,
                ContentType = file.Properties.ContentType
            });
        }
        Console.WriteLine("---------FILE NAMES-----------");
        foreach (var file in files)
        {
            Console.WriteLine(file.Name);
        }
        Console.WriteLine("---------FILE NAMES-----------");
        return files;
    }

    public async Task<BlobResponseDto> UploadAsync(IFormFile blob){
        Console.WriteLine("---------UploadAsync-----------");
        BlobResponseDto response = new();
        BlobClient client = _filesContainer.GetBlobClient(blob.FileName);

        await using (Stream? data = blob.OpenReadStream())
        {
            await client.UploadAsync(data);
        }

        response.Status = $"File {blob.FileName} Uploaded";
        response.Error = false;
        response.Blob.Uri = client.Uri.AbsoluteUri;
        response.Blob.Name = client.Name;

        return response;
    }

    public async Task<BlobDto?> DownloadAsync(string blobFileName){
        BlobClient file = _filesContainer.GetBlobClient(blobFileName);

        if (await file.ExistsAsync())
        {
            var data = await file.OpenReadAsync();
            Stream blobContent = data;

            var content = await file.DownloadContentAsync();

            string name = blobFileName;
            string contentType = content.Value.Details.ContentType;

            return new BlobDto{
                Content = blobContent,
                Name = name,
                ContentType = contentType
            };
        }
        else
            return null;
    }

    public async Task<BlobResponseDto> DeleteAsync(string blobFileName){
        BlobClient file = _filesContainer.GetBlobClient(blobFileName);

        await file.DeleteAsync();

        return new BlobResponseDto{
            Error = false,
            Status = "deleted"
            
        };
    }

}