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
            // Check if a file exists
            new TaskCheckFileExists(this.loggingService, null, @"C:\temp\thomas.xml", this.fileService).Execute();
            new TaskCheckFileExists(this.loggingService, null, @"C:\temp\thoma.xml", this.fileService).Execute();

            DirectoryInfo tempFolder = new DirectoryInfo(@"C:\temp");

            // Create a directory in C:\temp
            new TaskCreateDirectory(this.loggingService, null, this.fileService, tempFolder, "new_folder").Execute();
            await Task.Delay(500);

            // Delete the directory in C:\temp
            new TaskDeleteDirectory(this.loggingService, null, this.fileService, tempFolder, "new_folder").Execute();

            // Start a new Notepad process
            new TaskStartProcess(this.loggingService, null, @"C:\Windows\System32\notepad.exe", this.fileService).Execute();
            await Task.Delay(1000);

            // Kill the Notepad process
            new TaskKillProcess(this.loggingService, null, "notepad").Execute();
        }
    }
}
