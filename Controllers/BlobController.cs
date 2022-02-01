using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureStorage.Controllers
{
    [ApiController]
    [Route("/[Controller]")]
    public class BlobController : Controller
    {
        public string ConnectionString = "";
        public string ContainerName = "";
        public string BlobName = "";
        BlobContainerClient containerClient;
        BlobClient blobClient;
        public BlobController()
        {
            containerClient = new BlobContainerClient(ConnectionString, ContainerName);
            blobClient = containerClient.GetBlobClient(BlobName);
        }

        [HttpGet]
        public IActionResult DownloadBlob(string downloadPath)
        {
            try
            {
                blobClient.DownloadTo(downloadPath);
            }
            catch
            {
                return BadRequest();
            }
            return Ok();

        }

        public IActionResult UploadBlob(string filePath)
        {


            try
            {
                blobClient.Upload(filePath);
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }

        public IActionResult GetBlobs()
        {
            ICollection<string> blobList ;

            return Ok(containerClient.GetBlobs());

        }
    }
}
