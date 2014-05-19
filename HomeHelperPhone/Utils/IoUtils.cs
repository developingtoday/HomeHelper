﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Tasks;
using Windows.Storage;

namespace HomeHelperPhone.Utils
{
    public class IoUtils
    {
        public static async Task<string> SaveImage(Stream stream, string fileName)
        {
            using (var isolatedStorage=IsolatedStorageFile.GetUserStoreForApplication())
            {
                var folder = ApplicationData.Current.LocalFolder;
                if (!isolatedStorage.DirectoryExists("Photos"))
                {
                    isolatedStorage.CreateDirectory("Photos");
                }
                if (isolatedStorage.FileExists(Path.Combine("Photos", fileName)))
                {
                    isolatedStorage.DeleteFile(Path.Combine("Photos",fileName));
                }
                var photosFolder = Path.Combine(folder.Path, "Photos");
                var fileLocation = Path.Combine(photosFolder,fileName);
                var storageFile = await folder.CreateFileAsync(Path.Combine("Photos",fileName),CreationCollisionOption.ReplaceExisting);
               
                using (var outputStream=await storageFile.OpenStreamForWriteAsync())
                {
                    await stream.CopyToAsync(outputStream);
                    return fileLocation;
                }
            }
        }
        public static async Task<string> SaveImage(PhotoResult e)
        {
            return await SaveImage(e.ChosenPhoto, Path.GetFileName(e.OriginalFileName));
        }

        public static void DeleteImages(IEnumerable<string> imgToBeDelete)
        {
            if (!imgToBeDelete.Any()) return;
            using (var isolatedStorage=IsolatedStorageFile.GetUserStoreForApplication())
            {
                var files = isolatedStorage.GetFileNames().Select(a => new
                                                                           {
                                                                               Folder = Path.GetDirectoryName(a),
                                                                               FileName = Path.GetFileName(a),
                                                                               FullPath = a
                                                                           }).Where(a=>a.Folder.ToLower().Equals("photos")).ToList();
                foreach (var file in files)
                {
                    if ((imgToBeDelete.Any(a => a.ToLower().Equals(file.FileName.ToLower()))

                                                                  ||
                                                                  (imgToBeDelete.Any(
                                                                      a => a.ToLower().Equals(file.FullPath.ToLower())))))
                    {
                        isolatedStorage.DeleteFile(file.FullPath);
                    }
                }
            }
        }

        public static IEnumerable<string> GetFiles()
        {
            using (var isolatedStorage=IsolatedStorageFile.GetUserStoreForApplication())
            {
                return isolatedStorage.GetFileNames();
            }
        }

    }
}
