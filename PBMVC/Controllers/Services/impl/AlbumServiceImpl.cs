using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PicBook;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Core;
using Microsoft.WindowsAzure.Storage.Table;

namespace PBMVC.Controllers.Services.Impl
{
    public class AlbumServiceImpl : IAlbumService
    {
        public AlbumServiceImpl(String connectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            Albums = tableClient.GetTableReference("album");
            Shares = tableClient.GetTableReference("shares");

            Albums.CreateIfNotExistsAsync();
            Shares.CreateIfNotExistsAsync();
        }

        private CloudTable Albums;

        private CloudTable Shares;

        public Album GetAlbumById(Guid id)
        {
            TableQuery<Album> projectionQuery = new TableQuery<Album>().Where(
                    TableQuery.GenerateFilterCondition("id", QueryComparisons.Equal, id.ToString())
                );
            return Albums.ExecuteQuerySegmentedAsync<Album>(projectionQuery, null).Result.First();
        }

        public void EditAlbum(Album album)
        {
            TableOperation updateOperation = TableOperation.Replace(album);
            Albums.ExecuteAsync(updateOperation);
        }

        public void CreateAlbum(Album album)
        {
            TableOperation insertOperation = TableOperation.Insert(album);

            Albums.ExecuteAsync(insertOperation);
        }

        public void DeleteAlbum(Album album)
        {
            TableOperation deleteOperation = TableOperation.Delete(album);

            Albums.ExecuteAsync(deleteOperation);
        }

        public void ShareAlbum(Album album, User user)
        {
            Sharerelation relation = new Sharerelation(user, album);

            TableOperation insertOperation = TableOperation.Insert(relation);

            Shares.ExecuteAsync(insertOperation);
        }

        public void RevokeShare(Album album, User user)
        {
            TableQuery<Sharerelation> projectionQuery = new TableQuery<Sharerelation>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("userid", QueryComparisons.Equal, user.ID.ToString()),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("albumid", QueryComparisons.Equal, album.ID.ToString())
                ));

            Sharerelation share = Shares.ExecuteQuerySegmentedAsync<Sharerelation>(projectionQuery, null).Result.First();

            if (share != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(share);

                Shares.ExecuteAsync(deleteOperation);
            }
        }

        public IEnumerable<Album> ListAlbumsByUser(User user)
        {
            TableQuery<Album> projectionQuery = new TableQuery<Album>().Where(
                    TableQuery.GenerateFilterCondition("owner", QueryComparisons.Equal, user.ID.ToString())
                );
            IEnumerable<Album> albums = Albums.ExecuteQuerySegmentedAsync<Album>(projectionQuery, null).Result.ToList();

            return albums;
        }

    }
}
