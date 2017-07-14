using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ObjectPool
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 非泛型版本
            var tasks = new List<Task>();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            foreach(var index in Enumerable.Range(0,100000))
            {
                tasks.Add(new Task(() =>
                {
                    //获取一个实例
                    var obj = ObjectPool.GetInstance();

                    Console.WriteLine($"[{index}]\tobj:{obj.GetHashCode()}");

                    //释放实例
                    //ObjectPool.Release(obj);
                    obj.Release();  //扩展方式形式

                }));
            }

            tasks.ForEach(t =>
            {
                t.Start();
            });

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"运行时间：{sw.ElapsedMilliseconds / 1000}s");
            #endregion

            #region 泛型版本
            var tasks1 = new List<Task>();
            Stopwatch sw1 = new Stopwatch();
            sw1.Start();

            foreach (var index in Enumerable.Range(0, 100000))
            {
                tasks1.Add(new Task(() =>
                {
                    //获取一个实例
                    var dbConnection = ObjectPool<IDbConnection>.GetInstance();

                    Console.WriteLine($"[{index}]\tobj:{dbConnection?.GetHashCode()}");

                    //释放实例
                    ObjectPool<IDbConnection>.Release(dbConnection);

                }));
            }

            tasks1.ForEach(t =>
            {
                t.Start();
            });

            Task.WaitAll(tasks1.ToArray());

            Console.WriteLine($"运行时间：{sw1.ElapsedMilliseconds / 1000}s");
            #endregion


            Console.ReadLine();
        }
    }
}
