using System;


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
