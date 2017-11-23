using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PicBook;


namespace PBMVC.Controllers.Services
{
    public interface IAlbumService
    {

        Album GetAlbumById(Guid id);
        void EditAlbum(Album album);
        void CreateAlbum(Album album);
        void DeleteAlbum(Album album);
        void ShareAlbum(Album album, User user);
        void RevokeShare(Album album, User user);
        IEnumerable<Album> ListAlbumsByUser(User user);

    }
}
