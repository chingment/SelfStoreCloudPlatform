using Autofac;
using Lumos.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{

    public sealed class Cat
    {
        public event EventHandler Calling;

        public void Call()
        {
            Console.WriteLine("猫叫了...");
            if (Calling != null)//检查是否有事件注册  
                Calling(this, EventArgs.Empty);//调用事件注册的方法  
        }
    }

    public sealed class Mouser
    {
        public void Escape()
        {
            Console.WriteLine("老鼠逃跑了...");
        }

    }

    public sealed class Master
    {
        //发生猫叫时惊醒  
        public void Wakeup(object sender, EventArgs e)
        {
            Console.WriteLine("主人惊醒了...");
        }
    }


    public class A
    {
        public static int a = 1;
        public A()
        {
            a++;
            Console.WriteLine("A.a=" + a);
            Console.WriteLine("我是A");
        }

        public virtual void Show()
        {
            Console.WriteLine("我是A.Show");
        }

        public virtual void Cry()
        {
            Console.WriteLine("我是A.Cry");
        }
    }

    public class B : A
    {
        public B()
        {
            a++;
            Console.WriteLine("B.a=" + a);
            Console.WriteLine("我是B");
        }

        public override void Show()
        {
            //base.Show();
            Console.WriteLine("我是B.Show");
        }

        public new void Cry()
        {
            Console.WriteLine("我是B.Cry");
        }
    }

    public class Test
    {
        public void Show()
        {
            Console.WriteLine("FileName");
        }
    }

    public class Test2
    {
        public Test Test { get; set; }

        public void Show()
        {
            if (Test != null)
            {
                Test.Show();
            }
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
    }

    public interface IPersonRepository
    {
        IEnumerable<Person> GetAll();
        Person Get(int id);
        Person Add(Person item);
        bool Update(Person item);
        bool Delete(int id);
    }

    public class PersonRepository : IPersonRepository
    {
        List<Person> person = new List<Person>();

        public PersonRepository()
        {
            Add(new Person { Id = 1, Name = "joye.net1", Age = 18, Address = "中国上海" });
            Add(new Person { Id = 2, Name = "joye.net2", Age = 18, Address = "中国上海" });
            Add(new Person { Id = 3, Name = "joye.net3", Age = 18, Address = "中国上海" });
        }
        public IEnumerable<Person> GetAll()
        {
            return person;
        }
        public Person Get(int id)
        {
            return person.Find(p => p.Id == id);
        }
        public Person Add(Person item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            person.Add(item);
            return item;
        }
        public bool Update(Person item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            int index = person.FindIndex(p => p.Id == item.Id);
            if (index == -1)
            {
                return false;
            }
            person.RemoveAt(index);
            person.Add(item);
            return true;
        }
        public bool Delete(int id)
        {
            person.RemoveAll(p => p.Id == id);
            return true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int num = 100;
            Thread[] th = new Thread[num];
            for (int i = 0; i < num; i++)
            {
                Thread n = new Thread(DoWork);
                th[i] = n;
                th[i].Start();
            }
        }

        public static void DoWork()
        {

            //builder.RegisterModule(new LoggingModule());
            //builder.RegisterType<Test>();

            var s = new Test2();

            s.Test.Show();

            var builder = new ContainerBuilder();
            builder.RegisterType<Test>().As<Test>();
            //builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            var container = builder.Build();
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //builder.Register(t => new Test()).As<Test>();
            //builder.RegisterType<Test2>();
            //var container = builder.Build();

            Test2 test2 = container.Resolve<Test2>();
            //Test2 ee = new Test2();
            test2.Show();

            //string sn = SnUtil.Build(Lumos.Entity.Enumeration.BizSnType.Order2StockIn, "111");
            //Console.WriteLine("sn:" + sn);
        }
    }
}

