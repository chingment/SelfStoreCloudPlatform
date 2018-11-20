using System;
using System.Data.Entity;
using System.Linq;
using Lumos.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Lumos.DAL.AuthorizeRelay
{
    public class AuthorizeRelayDbContext : DbContext
    {

        //使用自定义连接串
        public static string GetEFConnctionString()
        {
            string enString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            return enString;
        }


        public AuthorizeRelayDbContext()
            : base(GetEFConnctionString())
        {
            // this.Configuration.ProxyCreationEnabled = false;
        }
        public DateTime GetDataBaseDateTime()
        {

            return this.Database.SqlQuery<DateTime>("select getdate()").FirstOrDefault();
        }

        public IDbSet<SysUser> SysUser { get; set; }

        public IDbSet<SysRole> SysRole { get; set; }

        public IDbSet<SysMenu> SysMenu { get; set; }

        public IDbSet<SysPermission> SysPermission { get; set; }

        public IDbSet<SysMenuPermission> SysMenuPermission { get; set; }

        public IDbSet<SysRoleMenu> SysRoleMenu { get; set; }

        //public IDbSet<SysRolePermission> SysRolePermission { get; set; }

        public IDbSet<SysUserRole> SysUserRole { get; set; }

        public IDbSet<SysUserLoginHistory> SysUserLoginHistory { get; set; }

        public IDbSet<SysStaffUser> SysStaffUser { get; set; }

        public IDbSet<SysClientUser> SysClientUser { get; set; }


        public IDbSet<SysProvinceCity> SysProvinceCity { get; set; }

        public IDbSet<SysClientCode> SysClientCode { get; set; }
        public IDbSet<SysOperateHistory> SysOperateHistory { get; set; }
        public IDbSet<SysPageAccessRecord> SysPageAccessRecord { get; set; }

        public IDbSet<SysAppInfo> SysAppInfo { get; set; }

        public IDbSet<SysSmsSendHistory> SysSmsSendHistory { get; set; }

        public IDbSet<SysItemCacheUpdateTime> SysItemCacheUpdateTime { get; set; }

        public IDbSet<SysMerchantUser> SysMerchantUser { get; set; }

        public IDbSet<BackgroundJob>  BackgroundJob { get; set; }

        public IDbSet<BackgroundJobLog> BackgroundJobLog { get; set; }
 

        public AuthorizeRelayDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            // this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DataSet GetPageReocrdByProc(QueryParam model)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@TableName", SqlDbType.VarChar,1000),
                    new SqlParameter("@ReturnFields", SqlDbType.VarChar,8000),
                    new SqlParameter("@PageSize", SqlDbType.Int,4),
                    new SqlParameter("@PageIndex", SqlDbType.Int,4),
                    new SqlParameter("@Where", SqlDbType.VarChar,8000),
                    new SqlParameter("@Orderfld", SqlDbType.VarChar,500),
                    new SqlParameter("@TotalRecord", SqlDbType.Int,4),
                    new SqlParameter("@TotalPage", SqlDbType.Int,4),
                    new SqlParameter("@Sql", SqlDbType.VarChar,8000)
            };
            parameters[0].Value = model.TableName;
            parameters[1].Value = model.ReturnFields;
            parameters[2].Value = model.PageSize;
            parameters[3].Value = model.PageIndex;
            if (model.Where == default(string) || model.Where.Trim() == "")
                parameters[4].Value = "1=1";
            else
                parameters[4].Value = model.Where;
            parameters[5].Value = model.Orderfld;
            parameters[6].Direction = ParameterDirection.Output;
            parameters[7].Direction = ParameterDirection.Output;
            parameters[8].Direction = ParameterDirection.Output;


            //调用分页存储过程
            SqlConnection conn = new SqlConnection(GetEFConnctionString());
            DataSet ds = new DataSet();
            conn.Open();
            SqlDataAdapter sqlDA = new SqlDataAdapter();

            SqlCommand command = new SqlCommand("Pro_StoredProcedurePage", conn);
            command.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
            {
                foreach (SqlParameter parameter in parameters)
                {
                    if (parameter != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value.
                        if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                            (parameter.Value == null))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        command.Parameters.Add(parameter);
                    }
                }
            }


            sqlDA.SelectCommand = command;
            sqlDA.Fill(ds);
            conn.Close();
            string a = parameters[6].Value.ToString();
            model.TotalRecord = int.Parse(parameters[6].Value.ToString());
            model.TotalPage = int.Parse(parameters[7].Value.ToString());
            model.Sql = parameters[8].Value.ToString();
            return ds;

        }

    }
}
