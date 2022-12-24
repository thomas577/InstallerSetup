using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerSetup.Models.Tasks
{
    public interface ITaskResult
    {
        bool IsSuccess { get; }
    }
}
