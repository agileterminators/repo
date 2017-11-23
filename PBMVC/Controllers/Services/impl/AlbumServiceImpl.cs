using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PicBook;


namespace PBMVC.Controllers.Services.Impl
{
    public class AlbumServiceImpl : DbContext, IAlbumService
    {
        private DbSet<Album> Albums { get; set; }

        private DbSet<Sharerelation> Shares { get; set; }

        public Album GetAlbumById(Guid id)
        {
            return Albums.Find(id);
        }

        public void EditAlbum(Album album)
        {
            Entry(album).State = EntityState.Modified;

            SaveChanges();
        }
        public void CreateAlbum(Album album)
        {
            Albums.Add(album);
            SaveChanges();
        }

        public void DeleteAlbum(Album album)
        {
            Albums.Remove(album);
            SaveChanges();
        }

        public void ShareAlbum(Album album, User user)
        {
            Shares.Add(new Sharerelation(user, album));

            SaveChanges();
        }

        public void RevokeShare(Album album, User user)
        {
            Sharerelation share = Shares.Find(album, user);

            Shares.Remove(share);
            SaveChanges();
        }

        public IEnumerable<Album> ListAlbumsByUser(User user)
        {
            return Albums.Where((x => x.Owner == user));
        }

    }
}
