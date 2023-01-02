using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;

namespace InstallerSetup.Services.FileSystem
{
    public class FileService : IFileService
    {
        public bool CheckFullPathExists(string fullPath)
        {
            return File.Exists(fullPath) || Directory.Exists(fullPath);
        }

        public bool CopyDirectoryRecursively(DirectoryInfo origin, DirectoryInfo target)
        {
            if (!this.CheckFullPathExists(origin.FullName)) return false;
            if (!this.CheckFullPathExists(target.FullName)) return false;
            foreach (DirectoryInfo dir in origin.GetDirectories()) this.CopyDirectoryRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in origin.GetFiles()) file.CopyTo(Path.Combine(target.FullName, file.Name));
            return true;
        }

        public bool CopyFile(string fullpathOrigin, string fullpathDestination)
        {
            if (!this.CheckFullPathExists(fullpathOrigin)) return false;
            string parentDestinationFolder = new FileInfo(fullpathDestination).Directory.FullName;
            if (!this.CheckFullPathExists(parentDestinationFolder)) return false;
            if (this.CheckFullPathExists(fullpathDestination)) return false;
            File.Copy(fullpathOrigin, fullpathDestination);
            return true;
        }

        public DirectoryInfo CreateDirectory(DirectoryInfo parent, string name)
        {
            if (!this.CheckFullPathExists(parent.FullName)) return null;
            return Directory.CreateDirectory(Path.Combine(parent.FullName, name));
        }

        public FileInfo CreateFile(DirectoryInfo parent, string name, byte[] bytes)
        {
            if (!this.CheckFullPathExists(parent.FullName)) return null;
            string fullpath = Path.Combine(parent.FullName, name);
            File.WriteAllBytes(fullpath, bytes);
            return new FileInfo(fullpath);
        }

        public bool DeleteDirectory(DirectoryInfo parent, string name)
        {
            if (!this.CheckFullPathExists(parent.FullName)) return false;
            string fullpath = Path.Combine(parent.FullName, name);
            if (!this.CheckFullPathExists(fullpath)) return false;
            Directory.Delete(fullpath);
            return true;
        }

        public bool DeleteDirectoryRecursively(DirectoryInfo parent, string name)
        {
            if (!this.CheckFullPathExists(parent.FullName)) return false;
            string fullpath = Path.Combine(parent.FullName, name);
            if (!this.CheckFullPathExists(fullpath)) return false;
            Directory.Delete(fullpath, recursive: true);
            return true;
        }

        public bool DeleteFile(DirectoryInfo parent, string name)
        {
            if (!this.CheckFullPathExists(parent.FullName)) return false;
            string fullpath = Path.Combine(parent.FullName, name);
            if (!this.CheckFullPathExists(fullpath)) return false;
            File.Delete(fullpath);
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

        public string GetFullPathFromRelativePath(DirectoryInfo directory, string relativePath)
        {
            return Path.Combine(directory.FullName, relativePath);
        }

        public byte[] ReadFile(string fullpath)
        {
            return File.ReadAllBytes(fullpath);
        }
    }
}
