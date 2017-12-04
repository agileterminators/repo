using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PicBook;
using PBMVC.Controllers.Services;

namespace PBMVC.Controllers.RestServices
{
    [Route("api/[controller]")]
    public class AlbumController : Controller
    {

        private IAlbumService albumService;
        private IUserService userService;

        public AlbumController(IAlbumService albumService, IUserService userService) {
            this.albumService = albumService;
            this.userService = userService;
        }

        [HttpPost]
        public void Create([FromBody] Album album) {
            User user = userService.LoggedIn(HttpContext);
            if (user == null) {
                album.Owner = user;
                albumService.CreateAlbum(album);
            } else {
                throw new OperationNotPermittedException();
            }
        }

        [HttpGet]
        [Route("/images")]
        public ICollection<Image> ListImages([FromQuery] String albumId) {

            Album album = albumService.GetAlbumById(new Guid(albumId));

            if (album == null)
            {
                throw new OperationNotPermittedException();
            }

            if (album.Visibility == Visibility.Public
                || (album.Visibility == Visibility.Shared
                && album.SharedWith.Any(x => x.user == userService.LoggedIn(HttpContext))))
            {
                return album.Images;
            } else {
                throw new OperationNotPermittedException();
            }
        }

        [HttpGet]
        [Route("/album")]
        public Album GetAlbum([FromQuery] String albumId) {
            Album album = albumService.GetAlbumById(new Guid(albumId));

            if (album == null)
            {
                throw new OperationNotPermittedException();
            }

            if (album.Visibility == Visibility.Public
                || (album.Visibility == Visibility.Shared
                && album.SharedWith.Any(x => x.user == userService.LoggedIn(HttpContext)))){
                    return album;
            } else {
                throw new OperationNotPermittedException();
            }
         }


        [HttpGet]
        [Route("/albums")]
        public IEnumerable<Album> ListAlbums([FromQuery] Guid userId) {
            User user = userService.GetUserById(userId);

            if (user == null) {
                throw new OperationNotPermittedException();
            }

            User loggedin = userService.LoggedIn(HttpContext);

            if (loggedin == null) {
                return user.Albums.Where(x => (x.Visibility == Visibility.Public));
            } else {
                return user.Albums.Where(x => ( (x.Visibility == Visibility.Public) || x.SharedWith.Any(y=>(y.user == loggedin))));
            }
        }

        [HttpDelete]
        public void Delete([FromQuery] String albumId) {
            Album album = albumService.GetAlbumById(new Guid(albumId));
            if (album != null && album.Owner == userService.LoggedIn(HttpContext)) {
                albumService.DeleteAlbum(album);
            } else {
                throw new OperationNotPermittedException();
            }
        }

        /*
        [HttpPut]
        public void Edit([FromBody] Album album) {
            Album oldAlbum = albumService.GetAlbumById(album.ID);
            if (album != null && album.Owner == userService.LoggedIn()) {
                oldAlbum.Title = album.Title;
                //...
                albumService.EditAlbum(oldAlbum);
            }
        }
        */

        [Route("/share-with")]
        [HttpPost]
        public void ShareWith([FromQuery]String albumId, [FromQuery]String userId) {
            Album album = albumService.GetAlbumById(new Guid(albumId));
            if (album.Owner == userService.LoggedIn(HttpContext)) {
                User user = userService.GetUserById(new Guid(userId));
                albumService.ShareAlbum(album, user);
            } else {
                throw new OperationNotPermittedException();
            }
        }

        [Route("/revoke-share")]
        [HttpPost]
        public void RevokeShare([FromQuery]String albumId, [FromQuery]String userId) {
            Album album = albumService.GetAlbumById(new Guid(albumId));
            if (album.Owner == userService.LoggedIn(HttpContext))
            {
                User user = userService.GetUserById(new Guid(userId));
                albumService.RevokeShare(album, user);
            } else {
                throw new OperationNotPermittedException();
            }
        }
    }
}
