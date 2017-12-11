using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PicBook;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
 
namespace PBMVC.Controllers.Services.Impl
{
    public class ImageServiceImpl: Controller, IImageService
    {

        public ImageServiceImpl(String connectionString) {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            Images = tableClient.GetTableReference("image");

            Images.CreateIfNotExistsAsync();
        }

        private CloudTable Images;

        public Image GetImageById(Guid id)
        {
            TableQuery<Image> projectionQuery = new TableQuery<Image>().Where(
                TableQuery.GenerateFilterCondition("id", QueryComparisons.Equal, id.ToString()));

            return Images.ExecuteQuerySegmentedAsync<Image>(projectionQuery, null).Result.First();
        }

        public void EditImage(Image image) {
            TableOperation updateOperation = TableOperation.Replace(image);
            Images.ExecuteAsync(updateOperation);
        }

        public void CreateImage(Image image) {
            TableOperation insertOperation = TableOperation.Insert(image);

            Images.ExecuteAsync(insertOperation);
        }

        public void DeleteImage(Image image) {
            TableOperation deleteOperation = TableOperation.Delete(image);

            Images.ExecuteAsync(deleteOperation);
        }

    }
}
