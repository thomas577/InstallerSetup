using InstallerSetup.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerSetup.Models.Tasks
{
    public interface ITask<out T> : ITask where T : ITaskResult
    {
        T Execute();
    }

    public interface ITask : INotifyPropertyChanged
    {
        TaskStatus Status { get; }

        int NestingLevel { get; }
    }
}
