using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.DAL
{
    public enum DatabaseType
    {
        SqlServer = 0,
        Oracle = 1
    }

    public interface IDBOption
    {
        IDbConnection Connection(string connectionString, DatabaseType databaseType);

        string ConnectionString { get; set; }
    }
}
