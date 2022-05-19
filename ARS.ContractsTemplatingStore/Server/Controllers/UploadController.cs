using System.Net;
using ARS.ContractsTemplatingStore.Shared.Models;
using ARS.ContractsTemplatingStore.Shared.Utils;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ARS.ContractsTemplatingStore.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly IWebHostEnvironment _env;
    private readonly BlobServiceClient _blobServiceClient;

    public UploadController(IWebHostEnvironment env, BlobServiceClient blobServiceClient )
    {
        _env = env;
        _blobServiceClient = blobServiceClient;
    }

    [HttpPost]
    public async Task Post([FromBody] FileObject[] files, CancellationToken cancelationToken)
    {
        foreach (var file in files)
        {
            var buf = Convert.FromBase64String(file.Base64data);
            if((file.State & 2) != 0)
                buf = await Compressor.DecompressAsync(buf, cancelationToken);
            
            string containerName = DateTime.Now.ToString("yyyy-MM-dd");
            BlobContainerClient containerClient = await _blobServiceClient.CreateBlobContainerAsync(containerName);
            using MemoryStream stream = new MemoryStream(buf);
            await containerClient.UploadBlobAsync($"{Guid.NewGuid().ToString()}-{file.FileName}", stream, cancelationToken);
            
            //await System.IO.File.WriteAllBytesAsync(Path.Combine(_env.ContentRootPath, Guid.NewGuid().ToString("N") + "-" + file.FileName), buf, cancelationToken);
        }
    }
}