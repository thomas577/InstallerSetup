using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InstallerSetup.Services.FileSystem
{
    public class FileService : IFileService
    {
        public bool CheckFullPathExists(string fullPath)
        {
            return File.Exists(fullPath) || Directory.Exists(fullPath);
        }

        public DirectoryInfo CreateDirectory(DirectoryInfo parent, string name)
        {
            if (!this.CheckFullPathExists(parent.FullName)) return null;
            return Directory.CreateDirectory(Path.Combine(parent.FullName, name));
        }

        public bool DeleteDirectory(DirectoryInfo parent, string name)
        {
            if (!this.CheckFullPathExists(parent.FullName)) return false;
            string fullpath = Path.Combine(parent.FullName, name);
            if (!this.CheckFullPathExists(fullpath)) return false;
            Directory.Delete(fullpath);
            return true;
        }

        public DirectoryInfo GetExeDirectory()
        {
            Uri location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            return new FileInfo(location.AbsolutePath).Directory;
        }

        public string GetFileNameFromFullPath(string fullPath)
        {
            FileInfo fileInfo = new FileInfo(fullPath);
            return fileInfo.Name;
        }

        public string GetFullPathFromRelativePath(string relativePath, DirectoryInfo directory)
        {
            return Path.Combine(directory.FullName, relativePath);
        }
    }
}
