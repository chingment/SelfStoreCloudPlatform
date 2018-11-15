using Lumos.Redis;


namespace Lumos.BLL.Biz
{
    
    public class RedisMq4GlobalProvider : RedisMqObject<RedisMq4GlobalModel>
    {
        protected override string MessageQueueKeyName { get { return "ReidsMq4GlobalProvider"; } }
        protected override bool IsTran { get { return false; } }

        //protected override string DB_Name { get { return "Order_DBName"; } }
    }
}
