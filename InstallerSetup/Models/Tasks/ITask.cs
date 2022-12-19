using InstallerSetup.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerSetup.Models.Tasks
{
    public interface ITask<out T, in U> : INotifyPropertyChanged
    {
        string Description { get; }

        TaskStatus Status { get; }

        int NestingLevel { get; }

        bool HasChildTasks { get; }

        T Execute(U context);
    }
}
