using InstallerSetup.Controls;
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
        private ObservableCollection<ILogViewerLine> logMessages;

        public MainWindowViewModel()
        {
            this.LogMessages = new ObservableCollection<ILogViewerLine>();
            this.FillWithMessages();
            this.ThomasClickCommand = new DelegateCommand(this.ThomasClick);
        }

        public ObservableCollection<ILogViewerLine> LogMessages
        {
            get { return this.logMessages; }
            set { this.SetProperty(ref this.logMessages, value); }
        }

        public ICommand ThomasClickCommand { get; }

        private void ThomasClick()
        {
            this.LogMessages = new ObservableCollection<ILogViewerLine>();
            this.FillWithMessages();
        }

        private void FillWithMessages()
        {
            Random random = new Random();
            this.LogMessages.Clear();
            for (int i = 0; i < random.Next(120); i++)
            {
                this.LogMessages.Add(new BasicLogViewerLine(DateTime.Now, "Hello ds fdsf sdfd fd f dsfs ESDFdfs fds fsdf df dsf sdf sd f dfgfdhbgfhfgd sf dd sf \r\nDFdgdfgfdg dfg fdg dfgfdssfe wf dfdgbfh dfg "));
            }
        }
    }
}
