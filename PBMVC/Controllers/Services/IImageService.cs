using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PicBook;
 
namespace PBMVC.Controllers.Services
{
    public interface IImageService 
    {
        Image GetImageById(Guid id);
        void EditImage(Image image);
        void CreateImage(Image image);
        void DeleteImage(Image image);
    }
}
