using System;


namespace Lumos.DAL
{
    public class DatabaseFactory
    {

        public static IDBOptionBySqlSentence GetIDBOptionBySql()
        {
        
            DBOptionBySqlSentenceProvider dbo = new DBOptionBySqlSentenceProvider();
            return dbo;
        }

    }
}
