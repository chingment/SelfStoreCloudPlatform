using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Task
{
    public class Launcher
    {
        public void Launch(string taskprovider)
        {
            Type type = Type.GetType(taskprovider);
            ITask task = (ITask)Activator.CreateInstance(type);
            task.Run();
        }
    }
}
