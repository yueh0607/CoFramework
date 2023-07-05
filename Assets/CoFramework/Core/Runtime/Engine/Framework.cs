using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace CoFramework
{
    public static class Framework
    {

        #region ModulesDriver
        private static DynamicDictionary<Type, IModule> modules = new DynamicDictionary<Type, IModule>();


        private static void CreateModule(Type type, CreateParameters parameters)
        {
            if (modules.ContainsKey(type)) return;
            var _depend = type.GetCustomAttribute<ModuleDependsAttribute>();
            if (_depend != null)
                for (int i = 0; i < _depend.Depends.Length; ++i)
                    if (!modules.ContainsKey(_depend.Depends[i]))
                        throw new InvalidOperationException($"Module {type.Name} depends on {_depend.Depends[i].Name}. Please first create and initialize it");

            modules.Enqueue(type, (IModule)Activator.CreateInstance(type));
            modules[type].OnCreate(parameters);
        }


        private static IModule GetModule(Type type)
        {
            if (!modules.ContainsKey(type)) return null;
            return modules[type];
        }


        private static void DestroyModule(Type type, bool exThrow = false)
        {
            if (!modules.ContainsKey(type))
            {
                if (exThrow)
                    throw new NullReferenceException("Destroying non-existent modules is not allowed");
                else return;
            }
            modules[type].OnDestroy();
            modules.Remove(type);
        }



        /// <summary>
        /// 检查是否存在模块，如果不存在则创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void CreateModule<T>(CreateParameters parameters) where T : IModule => CreateModule(typeof(T), parameters);

        /// <summary>
        /// 存在时返回模块，不存在时返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetModule<T>() where T : IModule => (T)GetModule(typeof(T));
        /// <summary>
        /// 检查是否存在模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool HasModule<T>() where T : IModule => modules.ContainsKey(typeof(T));
        /// <summary>
        /// 检查是否存在模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool HasModule(Type type) => modules.ContainsKey(type);


        /// <summary>
        /// 存在模块时返回，不存在时创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T GetOrCreateModule<T>(CreateParameters parameters) where T : IModule
        {
            T module = GetModule<T>();
            if (module == null) CreateModule<T>(parameters);
            module = GetModule<T>();
            return module;
        }
        /// <summary>
        /// 销毁模块，exThrow为true时，不存在则异常，否则不存在则静默处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void DestroyModule<T>(bool exThrow = false) where T : IModule => DestroyModule(typeof(T), exThrow);



        /// <summary>
        /// Update生命周期
        /// </summary>
        public static event Action Update = null;

        /// <summary>
        /// 上一帧到这一帧的时间间隔
        /// </summary>
        public static float deltaTime = 0;

        /// <summary>
        /// 时间流速
        /// </summary>
        public static float timeScale = 1;

        /// <summary>
        /// 驱动框架进行运行更新
        /// </summary>
        /// <param name="deltaTime">上一帧到这一帧的时间间隔(s)</param>
        public static void DriveUpdate(float deltaTime)
        {
            Framework.deltaTime = deltaTime * timeScale;
            modules.RefreshTraversalCount();
            int traversalCount = modules.TraversalCount;
            for (int i = 0; i < traversalCount; ++i)
            {
                if (modules.TryDequeue(out IModule module, out Type key))
                {
                    module.OnUpdate();
                    modules.TryEnqueue(key, module);
                }
            }
            Update?.Invoke();
        }

        #endregion
        #region GlobalPool

        private readonly static Dictionary<Type, Queue> pool = new Dictionary<Type, Queue>();
        /// <summary>
        /// 全局对象池对每个类型的最大容量，普通类型占用内存较少，可以统一调节
        /// </summary>
        public static int GlobalPoolMaxCount { get; set; } = 1000;
        private static Queue AddOrGetPool(Type type)
        {
            if (!pool.ContainsKey(type)) pool.Add(type, new Queue());
            return pool[type];
        }

        /// <summary>
        /// 回收到全局对象池，不做任何处理
        /// </summary>
        /// <param name="item"></param>
        public static void GlobalRecycle(object item)
        {
            Type type = item.GetType();
            var queue = AddOrGetPool(type);
            if (queue.Count >= GlobalPoolMaxCount) return;
            queue.Enqueue(item);
        }

        /// <summary>
        /// 从全局对象池申请，创建为同步，空池触发
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GlobalAllocate(Type type)
        {
            var queue = AddOrGetPool(type);
            return queue.Count == 0 ? Activator.CreateInstance(type) : queue.Dequeue();
        }
        /// <summary>
        /// 从全局对象池申请，创建为同步，空池触发
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GlobalAllocate<T>() => (T)GlobalAllocate(typeof(T));
        /// <summary>
        /// 对全局对象池下达减持命令，可以指定类型，可以指定数量，不指定则为全部
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static void GlobalReduce(Type type = null, int count = -1)
        {
            if (type == null) pool.Clear();
            else
            {
                if (!pool.ContainsKey(type)) return;
                if (count < 0) pool.Remove(type);
                else
                {
                    var queue = pool[type];
                    count = Math.Min(count, queue.Count);
                    for (int i = 0; i < count; ++i) queue.Dequeue();
                }

            }
        }
        /// <summary>
        /// 对全局对象池下达减持到命令，可以指定类型，可以指定数量，不指定则为全部
        /// 该命令使得全局对象池不超过指定容量
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static void GlobalReduceTo(Type type = null, int count = 1000)
        {
            if (type == null)
                foreach (var item in pool)
                {
                    int reduceCount = item.Value.Count - count;
                    if (reduceCount <= 0) continue;
                    GlobalReduce(item.Key, reduceCount);
                }
            else if (pool.ContainsKey(type))
            {
                int reduceCount = pool[type].Count - count;
                if (reduceCount <= 0) return;
                GlobalReduce(type, reduceCount);
            }
        }

        public static void CreateModule<T>()
        {
            throw new NotImplementedException();
        }
        #endregion

    }

}

