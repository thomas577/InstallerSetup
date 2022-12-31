﻿using InstallerSetup.Controls;
using InstallerSetup.Models.Tasks;
using InstallerSetup.Services.FileSystem;
using InstallerSetup.Services.Logging;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            this.ThomasClickCommand = new DelegateCommand(this.ThomasClick);
        }

        public ILogViewerLineList LogLines { get; }

        public ICommand ThomasClickCommand { get; }

        private void ThomasClick()
        {
            Random random = new Random();
            this.LogLines.Clear();
            for (int i = 0; i < random.Next(120); i++)
            {
                this.loggingService.Log("Hello ds fdsf sdfd fd f dsfs ESDFdfs fds fsdf df dsf sdf sd f dfgfdhbgfhfgd sf dd sf \r\n" +
                    "DFdgdfgfdg dfg fdg dfgfdssfe wf dfdgbfh dfg ", LoggingType.Error, 2);
            }

            TaskCheckFileExists task = new TaskCheckFileExists(this.loggingService, null, @"C:\temp\thomas.xml", this.fileService);
            task.Execute();
        }
    }
}
