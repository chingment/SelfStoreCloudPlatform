using System;

namespace Lumos.BLL.Biz
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
