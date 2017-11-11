using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PicBook;
 
namespace PBMVC.Controllers.Services
{
    public class ImageService: DbContext 
    {

        private DbSet<Image> Images { get; set;  }

        public Image GetImageById(Guid id) {
            return Images.Find(id);
        }
        public void EditImage(Image image) {
            Entry(image).State = EntityState.Modified;
            SaveChanges();
        }
        public void CreateImage(Image image) {
            Images.Add(image);
            SaveChanges();
        }
        public void DeleteImage(Image image) {
            Images.Remove(image);
            SaveChanges();
        }

    }
}
