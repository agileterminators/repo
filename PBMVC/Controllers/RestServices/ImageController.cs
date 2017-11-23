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
    public class PictureController:Controller
    {

        private IUserService userService;
        private IImageService imageService;
        private IFileService fileService;
        private IAlbumService albumService;

        public PictureController(IUserService userService, IImageService imageService, IFileService fileService, IAlbumService albumService)
        {
            this.fileService = fileService;
            this.imageService = imageService;
            this.userService = userService;
            this.albumService = albumService;
        }

        [HttpGet]
        public byte[] GetImageByID([FromQuery] Guid id)
        {
            Image image = imageService.GetImageById(id);
            if (image!= null && (image.album.SharedWith.Any(x => x.user==userService.LoggedIn(HttpContext))
                || image.album.Visibility == Visibility.Public))
            {
                return fileService.ReadFile(image.FilePath);
            } else {
                throw new OperationNotPermittedException();
            }  
        }

        [HttpPost]
        public void Upload([FromBody] byte[] bytes, [FromQuery] String title, [FromQuery] String albumId) {
            Album album = albumService.GetAlbumById(new Guid(albumId));
            if (album != null && album.Owner==userService.LoggedIn(HttpContext))
            {
                Image image = new Image();
                image.Title = title;
                Guid guid = new Guid();
                image.FilePath = guid.ToString();
                fileService.SaveFile(guid.ToString(), bytes);
                imageService.CreateImage(image);
                album.Images.Add(image);
                albumService.EditAlbum(album);   
            } else {
                throw new OperationNotPermittedException();
            }
        }

        [HttpDelete]
        public void Delete([FromQuery]Guid imageId)
        { 
            Image image = imageService.GetImageById(imageId);
            if (image != null && image.album.Owner == userService.LoggedIn(HttpContext))
            {
                imageService.DeleteImage(image);
                fileService.DeleteFile(image.FilePath);
            } else {
                throw new OperationNotPermittedException();
            }
        }

        [HttpPut]
        public void edit([FromBody] Image image)
        {
            Image oldimg = imageService.GetImageById(image.ID);
            if (oldimg != null && oldimg.album.Owner == userService.LoggedIn(HttpContext)) 
            {
                oldimg.Title = image.Title;
                imageService.EditImage(oldimg);
            } else {
                throw new OperationNotPermittedException();
            }
        }

    }
}
