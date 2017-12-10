using PicBook;
using PBMVC.Controllers.Services;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace PBMVC.Tests
{

    class MockAlbumService : IAlbumService
    {
        public void CreateAlbum(Album album)
        {
            throw new NotImplementedException();
        }

        public void DeleteAlbum(Album album)
        {
            throw new NotImplementedException();
        }

        public void EditAlbum(Album album)
        {
            throw new NotImplementedException();
        }

        public Album GetAlbumById(Guid id)
        {
            return MockObjects.Albums.Find(x => x.ID == id);
        }

        public IEnumerable<Album> ListAlbumsByUser(User user)
        {
            throw new NotImplementedException();
        }

        public void RevokeShare(Album album, User user)
        {
            throw new NotImplementedException();
        }

        public void ShareAlbum(Album album, User user)
        {
            throw new NotImplementedException();
        }
    }

    class MockFileService : IFileService
    {
        public void DeleteFile(string name)
        {
            throw new NotImplementedException();
        }

        public byte[] ReadFile(string name)
        {
            throw new NotImplementedException();
        }

        public void SaveFile(string name, byte[] file)
        {
            throw new NotImplementedException();
        }
    }

    class MockImageService : IImageService
    {
        public void CreateImage(Image image)
        {
            throw new NotImplementedException();
        }

        public void DeleteImage(Image image)
        {
            throw new NotImplementedException();
        }

        public void EditImage(Image image)
        {
            throw new NotImplementedException();
        }

        public Image GetImageById(Guid id)
        {
            throw new NotImplementedException();
        }
    }

    public class MockUserService : IUserService
    {

        private User loggedin;

        public void CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public void EditUser(User user)
        {
            throw new NotImplementedException();
        }

        public User GetUserById(Guid userid)
        {
            throw new NotImplementedException();
        }

        public User LoggedIn(HttpContext context)
        {
            return loggedin;
        }

        public void ImpersonateUser(User user) {
            loggedin = user;
        }
    }

    public static class MockObjects
    {

        public static readonly User User1;
        public static readonly User User2;
        public static readonly User User3;

        public static readonly Album Album1;

        public static readonly List<User> Users;
        public static readonly List<Album> Albums;

        static MockObjects()
        {
            User1 = new User(Guid.NewGuid(), "user1", "user1@test.com");
            User2 = new User(Guid.NewGuid(), "user2", "user2@test.com");
            User3 = new User(Guid.NewGuid(), "user3", "user3@test.com");

            Album1 = new Album(Guid.NewGuid(), "Album1");
            Album1.Owner = User1;
            Album1.ShareWith(User2);

            Users = new List<User>();
            Users.Add(User1);
            Users.Add(User2);
            Users.Add(User3);

            Albums = new List<Album>();
            Albums.Add(Album1);
        }

    }

}