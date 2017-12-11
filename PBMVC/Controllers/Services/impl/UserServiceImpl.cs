using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PicBook;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace PBMVC.Controllers.Services.Impl
{
    public class UserServiceImpl: Controller, IUserService
    {

        private const String LOGGED_IN_USER = "LOGGED_IN_USER";

        public UserServiceImpl(String connectionString) {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            Users = tableClient.GetTableReference("pbookers");

            Users.CreateIfNotExistsAsync();
        }

        private CloudTable Users { get; set; }
        
        public User GetUserById(Guid userid) {
            TableQuery<User> projectionQuery = new TableQuery<User>().Where(
                TableQuery.GenerateFilterCondition("id", QueryComparisons.Equal, userid.ToString()));

            return Users.ExecuteQuerySegmentedAsync<User>(projectionQuery, null).Result.First();
        }

        public void EditUser(User user) {
            TableOperation updateOperation = TableOperation.Replace(user);
            Users.ExecuteAsync(updateOperation);
        }

        public void CreateUser(User user){
            TableOperation insertOperation = TableOperation.Insert(user);
            Users.ExecuteAsync(insertOperation);
        }

        public User LoggedIn(HttpContext context) {
            string user_id = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            TableQuery<User> projectionQuery = new TableQuery<User>().Where(
                TableQuery.GenerateFilterCondition("user_id", QueryComparisons.Equal, user_id));

            return Users.ExecuteQuerySegmentedAsync<User>(projectionQuery, null).Result.First();
        }

    }
}
