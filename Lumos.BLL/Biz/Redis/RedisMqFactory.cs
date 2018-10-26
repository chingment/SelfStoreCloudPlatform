using System;

namespace Lumos.BLL
{
    public static class RedisMqFactory
    {
        public static RedisMq4GlobalProvider Global
        {
            get
            {
                return new RedisMq4GlobalProvider();
            }
        }
    }
}
