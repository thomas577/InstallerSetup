using InstallerSetup.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerSetup.Services.Logging
{
    public class LoggingLineList : ObservableCollection<ILogViewerLine>, ILogViewerLineList
    {
    }
}
