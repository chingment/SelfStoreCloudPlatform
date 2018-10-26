using System;


namespace Lumos.BLL.Task
{
    public static class Task4Factory
    {
        public static Launcher Launcher
        {
            get
            {
                return new Launcher();
            }
        }

        public static Task4Tim2GlobalProvider Global
        {
            get
            {
                return new Task4Tim2GlobalProvider();
            }
        }
    }
}
