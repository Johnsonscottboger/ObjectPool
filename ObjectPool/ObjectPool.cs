using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPool
{
    /// <summary>
    /// 对象池
    /// </summary>
    public static class ObjectPool
    {
        /// <summary>
        /// 对象池中最小数量
        /// </summary>
        private const int _minSize = 2;

        /// <summary>
        /// 对象池中最大数量
        /// </summary>
        private const int _maxSize = 100;

        /// <summary>
        /// 对象池
        /// </summary>
        private static ConcurrentQueue<object> _connQueue = new ConcurrentQueue<object>();
        
        /// <summary>
        /// 静态构造方法,初始化对象池
        /// </summary>
        static ObjectPool()
        {
            foreach (var index in Enumerable.Range(0, _minSize))
            {
                _connQueue.Enqueue(CreateNewInstance());
            }
        }

        /// <summary>
        /// 从对象池中获取一个可用对象
        /// </summary>
        /// <returns>可用对象</returns>
        public static Object GetInstance()
        {
            Object instance = null;
            if (!_connQueue.TryDequeue(out instance))
            {
                instance = CreateNewInstance();
            }
            return instance;
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="obj">释放的对象</param>
        public static void Release(this Object obj)
        {
            if (obj == null)
                return;
            if (_connQueue.Count < _maxSize)
            {
                if (_connQueue.Any(p => p == obj))
                {
                    //抛出此异常,表明此对象池实现不正确
                    throw new Exception("同一个对象被同时使用");
                }
                _connQueue.Enqueue(obj);
            }
        }

        /// <summary>
        /// 创建新对象
        /// </summary>
        /// <returns>一个新的对象</returns>
        private static Object CreateNewInstance()
        {
            //具体创建一个新对象的方法
            return new object();
        }
    }

    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    public static class ObjectPool<T>
    {
        /// <summary>
        /// 对象池中最小数量
        /// </summary>
        private const int _minSize = 2;

        /// <summary>
        /// 对象池中最大数量
        /// </summary>
        private const int _maxSize = 100;

        /// <summary>
        /// 对象池
        /// </summary>
        private static ConcurrentQueue<T> _connQueue = new ConcurrentQueue<T>();

        /// <summary>
        /// 静态构造方法,初始化对象池
        /// </summary>
        static ObjectPool()
        {
            foreach (var index in Enumerable.Range(0, _minSize))
            {
                _connQueue.Enqueue(CreateNewInstance());
            }
        }

        /// <summary>
        /// 从对象池中获取一个可用对象
        /// </summary>
        /// <returns>可用对象</returns>
        public static T GetInstance()
        {
            T instance = default(T);
            if (!_connQueue.TryDequeue(out instance))
            {
                instance = CreateNewInstance();
            }
            return instance;
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="obj">释放的对象</param>
        public static void Release(T obj)
        {
            if (obj == null)
                return;
            if (_connQueue.Count < _maxSize)
            {
                if (_connQueue.Any(p => p.Equals(obj)))
                {
                    //抛出此异常,表明此对象池实现不正确
                    throw new Exception("同一个对象被同时使用");
                }
                _connQueue.Enqueue(obj);
            }
        }

        /// <summary>
        /// 创建新对象
        /// </summary>
        /// <returns>一个新的对象</returns>
        private static T CreateNewInstance()
        {
            //TODO:具体创建一个新对象的方法
            return default(T);
        }
    }
}
