using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace PBMVC.Controllers.Services
{
    public interface IFileService
    {
        void SaveFile(string name, byte[] file);
        Byte[] ReadFile(string name);
        void DeleteFile(string name);
    }
}
