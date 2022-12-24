using InstallerSetup.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerSetup.Models.Tasks
{
    public interface ITask<out T> : INotifyPropertyChanged
    {
        TaskStatus Status { get; }

        int NestingLevel { get; }

        T Execute();
    }
}
