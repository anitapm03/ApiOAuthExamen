using ApiOAuthExamen.Models;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace ApiOAuthExamen.Services
{
    public class ServiceStorageBlobs
    {
        private BlobServiceClient client;

        public ServiceStorageBlobs(BlobServiceClient client)
        {
            this.client = client;
        }

        //metodo para mostrar los containers
        public async Task<List<string>>
            GetContainersAsync()
        {
            List<string> containers = new List<string>();
            await foreach
                (BlobContainerItem item in this.client.GetBlobContainersAsync())
            {
                containers.Add(item.Name);
            }
            return containers;
        }

        

        //metodo para recuperar el acceso al container
        public string GetContainerUrl(string containerName)
        {
            // Recuperamos el cliente del contenedor
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerName);

            // Obtenemos la URL del contenedor
            return containerClient.Uri.AbsoluteUri;
        }

        //METODO PARA RECUPERAR UN BLOB
        public async Task<string> GetBlobUrlAsync
            (string containerName, string blobName)
        {
            // Recuperamos el cliente del contenedor
            BlobContainerClient containerClient = 
                this.client.GetBlobContainerClient(containerName);

            // Creamos el BlobClient para el blob específico
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Obtenemos la URL del blob
            BlobProperties properties = await blobClient.GetPropertiesAsync();
            return blobClient.Uri.AbsoluteUri;
        }

        
    }
}

