using System;


namespace Lumos
{
    public class GuidUtil
    {
        public static string New()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
        public static string Empty()
        {
            return Guid.Empty.ToString().Replace("-", "");
        }
    }
}
