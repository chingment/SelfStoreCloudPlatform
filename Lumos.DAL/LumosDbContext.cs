using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.DAL
{

    public class LumosDbContext : AuthorizeRelayDbContext
    {

        //public FxDbContext()
        //    : base("DefaultConnection")
        //{
        //   // this.Configuration.ProxyCreationEnabled = false;
        //}



        public IDbSet<Machine> Machine { get; set; }
        public IDbSet<Merchant> Merchant { get; set; }
        public IDbSet<MerchantMachine> MerchantMachine { get; set; }
        public IDbSet<MachineBanner>  MachineBanner { get; set; }
        public IDbSet<ProductKind> ProductKind { get; set; }
        public IDbSet<ProductKindSku> ProductKindSku { get; set; }
        public IDbSet<ProductSku> ProductSku { get; set; }

        public IDbSet<MachineStock> MachineStock { get; set; }
        public IDbSet<MachineStockLog> MachineStockLog { get; set; }
        public IDbSet<Store> Store { get; set; }
        public IDbSet<StoreMachine> StoreMachine { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }


    public class FxContextDatabaseInitializerForCreateDatabaseIfNotExists : CreateDatabaseIfNotExists<LumosDbContext>
    {
        protected override void Seed(LumosDbContext context)
        {
            base.Seed(context);
        }

        public List<SysPermission> GetPermissionList(PermissionCode permission)
        {
            Type t = permission.GetType();
            List<SysPermission> list = new List<SysPermission>();
            list = GetPermissionList(t, list);
            return list;
        }

        private List<SysPermission> GetPermissionList(Type t, List<SysPermission> list)
        {
            if (t.Name != "Object")
            {
                System.Reflection.FieldInfo[] properties = t.GetFields();
                foreach (System.Reflection.FieldInfo property in properties)
                {
                    string pId = "0";
                    object[] typeAttributes = property.GetCustomAttributes(false);
                    foreach (PermissionCodeAttribute attribute in typeAttributes)
                    {
                        pId = attribute.PId;
                    }
                    object id = property.GetValue(null);
                    string name = property.Name;
                    SysPermission model = new SysPermission();
                    model.Id = id.ToString();
                    model.Name = name;
                    model.PId = pId;
                    list.Add(model);
                }
                list = GetPermissionList(t.BaseType, list);
            }
            return list;
        }

    }

}
