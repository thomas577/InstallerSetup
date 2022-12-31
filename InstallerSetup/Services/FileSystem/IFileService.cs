using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerSetup.Services.FileSystem
{
    public interface IFileService
    {
        /// <summary>Checks if the directory or file full path exists already</summary>
        bool CheckFullPathExists(string fullPath);
        
        /// <summary>Returns the directory where the current executable was launched</summary>
        DirectoryInfo GetExeDirectory();

        /// <summary>Returns a full path constructed by applying the relative path to the provided directory as a starting point</summary>
        string GetFullPathFromRelativePath(string relativePath, DirectoryInfo directory);

        /// <summary>Extracts the filename, e.g. 'readme.txt' from a full path such as 'c:\path\to\readme.txt'</summary>
        string GetFileNameFromFullPath(string fullPath);

        /// <summary>Creates a new directory with the given name (if it does not already exist) inside the parent directory</summary>
        DirectoryInfo CreateDirectory(DirectoryInfo parent, string name);

        /// <summary>Deletes a directory from a parent directory. Throws an exception if the directory to delete is not empty</summary>
        bool DeleteDirectory(DirectoryInfo parent, string name);
    }
}
