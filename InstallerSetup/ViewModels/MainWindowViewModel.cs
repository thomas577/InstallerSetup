using InstallerSetup.Controls;
using InstallerSetup.Models.Tasks;
using InstallerSetup.Services.FileSystem;
using InstallerSetup.Services.Logging;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InstallerSetup.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly ILoggingService loggingService;
        private readonly IFileService fileService;

        public MainWindowViewModel(ILoggingService loggingService, IFileService fileService)
        {
            this.loggingService = loggingService;
            this.fileService = fileService;
            this.LogLines = this.loggingService.LogLines;
            this.StartInstallCommand = new DelegateCommand(this.StartInstall);
        }

        public ILogViewerLineList LogLines { get; }

        public ICommand StartInstallCommand { get; }

        private async void StartInstall()
        {
            DirectoryInfo tempFolder = new DirectoryInfo(@"C:\temp");

            // Create a file with some content in C:\temp
            new TaskCreateFile(this.loggingService, null, this.fileService, tempFolder, "thomas.xml", Encoding.ASCII.GetBytes("abc")).Execute();

            // Check if a file exists
            new TaskCheckFileExists(this.loggingService, null, @"C:\temp\thomas.xml", this.fileService).Execute();
            new TaskCheckFileExists(this.loggingService, null, @"C:\temp\thoma.xml", this.fileService).Execute();

            // Reads the content of a file
            new TaskReadFile(this.loggingService, null, this.fileService, @"C:\temp\thomas.xml").Execute();

            // Create a directory in C:\temp
            new TaskCreateDirectory(this.loggingService, null, this.fileService, tempFolder, "new_folder").Execute();
            await Task.Delay(500);

            // Delete the directory in C:\temp
            new TaskDeleteDirectory(this.loggingService, null, this.fileService, tempFolder, "new_folder").Execute();

            // Delete a file in C:\temp
            new TaskDeleteFile(this.loggingService, null, this.fileService, tempFolder, "thomas.xml").Execute();

            // Start a new Notepad process
            new TaskStartProcess(this.loggingService, null, @"C:\Windows\System32\notepad.exe", this.fileService).Execute();
            await Task.Delay(1000);

            // Kill the Notepad process
            new TaskKillProcess(this.loggingService, null, "notepad").Execute();

            // Try to copy a whole folder hierarchy at once...
            // 1. Create the folder hierarchy
            DirectoryInfo originFolder = new TaskCreateDirectory(this.loggingService, null, this.fileService, tempFolder, "origin_folder").Execute().DirectoryCreated;
            new TaskCreateFile(this.loggingService, null, this.fileService, originFolder, "a.txt", Encoding.ASCII.GetBytes("a")).Execute();
            DirectoryInfo originSubFolder = new TaskCreateDirectory(this.loggingService, null, this.fileService, originFolder, "subfolder").Execute().DirectoryCreated;
            new TaskCreateFile(this.loggingService, null, this.fileService, originSubFolder, "b.txt", Encoding.ASCII.GetBytes("b")).Execute();

            // 2. Copy to a new folder
            new TaskCopyDirectoryRecursively(this.loggingService, null, this.fileService, originFolder, tempFolder, "target_folder").Execute();

            // 3. Check it was created alright
            new TaskCheckFileExists(this.loggingService, null, @"C:\temp\target_folder\a.txt", this.fileService).Execute();
            new TaskCheckFileExists(this.loggingService, null, @"C:\temp\target_folder\subfolder\b.txt", this.fileService).Execute();

            // 4. Clean up
            new TaskDeleteDirectoryRecursively(this.loggingService, null, this.fileService, tempFolder, "origin_folder").Execute();
            new TaskDeleteDirectoryRecursively(this.loggingService, null, this.fileService, tempFolder, "target_folder").Execute();
        }
    }
}
