using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using  Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace PBMVC.Controllers.Services.Impl
{
    public class FileServiceImpl : Controller, IFileService
    {
        public FileServiceImpl() {
        }

        string foldername = "C:\\pbook"; //need to be implemented 

        public void SaveFile(string name, byte[] file) {
            FileStream output = new FileStream(foldername + "\\" + name, FileMode.Create);
            output.WriteAsync(file, 0, file.Length);
        }

        public Byte[] ReadFile(string name) {
            return System.IO.File.ReadAllBytes(foldername + "\\" + name);
        }

        public void DeleteFile(string name) {
            System.IO.File.Delete(foldername + "\\" + name);
        }


    }
}
