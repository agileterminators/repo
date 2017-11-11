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

        private AlbumService albumService;
        private UserService userService;

        public AlbumController(AlbumService albumService, UserService userService) {
            this.albumService = albumService;
            this.userService = userService;
        }

        [HttpPost]
        public void Create([FromBody] Album album) {
            album.Owner = userService.LoggedIn(HttpContext);
            albumService.CreateAlbum(album);
        }

        [HttpGet]
        [Route("/images")]
        public ICollection<Image> ListImages([FromQuery] String albumid) {

            Album album = albumService.GetAlbumById(new Guid(albumid));

            if (album == null)
            {
                return null;
            }

            if (album.Visibility == Visibility.Public || (album.Visibility == Visibility.Shared && album.SharedWith.Any(x => x.user == userService.LoggedIn(HttpContext))))
            {
                return album.Images;
            }

            return null;
        }

        [HttpGet]
        [Route("/albums")]
        public ICollection<Album> ListAlbums([FromQuery] String userid) { return null;  }

        [HttpDelete]
        public void Delete([FromQuery] String albumId) {
            Album album = albumService.GetAlbumById(new Guid(albumId));
            if (album != null && album.Owner == userService.LoggedIn(HttpContext)) {
                albumService.DeleteAlbum(album);
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
            }
        }
    }
}
