using Azure.Storage;
using Azure.Storage.Files.DataLake;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureStorage.Controllers
{
    public class DatalakeController : Controller
    {
        DataLakeServiceClient datalakeServiceClient;
        public DatalakeController()
        {
            StorageSharedKeyCredential sharedKeyCredential =
        new StorageSharedKeyCredential("accountName", "accountKey");
            string dfsUri = "https://" + "accountName" + ".dfs.core.windows.net";

            datalakeServiceClient = new DataLakeServiceClient
                (new Uri(dfsUri), sharedKeyCredential);

        }
        public async Task<DataLakeFileSystemClient> CreateContainer(string containerName)
        {
            return await datalakeServiceClient.CreateFileSystemAsync(containerName);
            
        }

        public async Task<DataLakeDirectoryClient> CreateDirectory(string containerName, string directoryName)
        {
            DataLakeFileSystemClient fileSystemClient = datalakeServiceClient.GetFileSystemClient(containerName);
            return await fileSystemClient.CreateDirectoryAsync(directoryName);
        }

        public IActionResult DeleteDirectory(string containerName, string directoryName)
        {
            DataLakeFileSystemClient fileSystemClient = datalakeServiceClient.GetFileSystemClient(containerName);
            DataLakeDirectoryClient directoryClient = fileSystemClient.GetDirectoryClient(directoryName);
            directoryClient.Delete();

            return Ok();
        }

        public async Task UploadFile(string fileSystem, string directoryName, string filePath)
        {
            DataLakeFileSystemClient fileSystemClient = datalakeServiceClient.GetFileSystemClient(fileSystem);
            
            DataLakeDirectoryClient directoryClient =
            fileSystemClient.GetDirectoryClient(directoryName);

            DataLakeFileClient fileClient = await directoryClient.CreateFileAsync("uploaded-file.txt");

            FileStream fileStream =
                System.IO.File.OpenRead(filePath);            

            long fileSize = fileStream.Length;

            await fileClient.UploadAsync(fileStream);

            await fileClient.FlushAsync(position: fileSize);

        }
    }
}
