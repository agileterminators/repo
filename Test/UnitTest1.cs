using System;
using Xunit;
using PicBook;
using PBMVC.Controllers.RestServices;
using PBMVC.Controllers.Services;

namespace PBMVC.Tests
{
    public class UnitTest1
    {

        private MockAlbumService albumService;
        private MockFileService fileService;
        private MockImageService imageService;
        private MockUserService userService;

        private AlbumController albumController;
        private PictureController pictureController;
        private UserController userController;

        public UnitTest1()
        {
            albumService = new MockAlbumService();
            fileService = new MockFileService();
            imageService = new MockImageService();
            userService = new MockUserService();
            albumController = new AlbumController(albumService, userService);
            pictureController = new PictureController(userService, imageService, fileService, albumService);
            userController = new UserController(userService);
        }

        [Fact]
        public void testNoPermissionToViewAlbum()
        {
            userService.ImpersonateUser(MockObjects.User3);

            var exception = Record.Exception(() =>
                albumController.GetAlbum(MockObjects.Album1.ID.ToString())
                );

            Assert.IsType(typeof(OperationNotPermittedException), exception);
        }

        [Fact]
        public void testViewAlbum()
        {
            userService.ImpersonateUser(MockObjects.User2);

            albumController.GetAlbum(MockObjects.Album1.ID.ToString());
        }

        private Album copyAlbum(Album album) {
            Album newInstance = new Album();
            newInstance.ID = album.ID;
            newInstance.Title = album.Title;
            newInstance.Created = album.Created;
            newInstance.Images = album.Images;
            newInstance.IsActive = album.IsActive;
            newInstance.Owner = album.Owner;
            newInstance.SharedWith = album.SharedWith;
            newInstance.Visibility = album.Visibility;

            return newInstance;
        }
    }
}
