using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoFramework
{
    public static class Framework
    {
        private static DynamicDictionary<Type, IModule> modules = new DynamicDictionary<Type, IModule>();


        private static void CreateModule(Type type, CreateParameters parameters)
        {
            if (modules.ContainsKey(type)) return;
            modules.Enqueue(type, (IModule)Activator.CreateInstance(type));
            modules[type].OnCreate(parameters);
        }


        private static IModule GetModule(Type type)
        {
            if (!modules.ContainsKey(type)) return null;
            return modules[type];
        }


        private static void DestroyModule(Type type)
        {
            if (!modules.ContainsKey(type)) throw new NullReferenceException("Destroying non-existent modules is not allowed");
            modules[type].OnDestroy();
            modules.Remove(type);
        }



        /// <summary>
        /// 检查是否存在模块，如果不存在则创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void CreateModule<T>(CreateParameters parameters) where T : IModule => CreateModule(typeof(T), parameters);

        /// <summary>
        /// 检查是否存在模块，存在时返回，不存在时创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetModule<T>() where T : IModule => (T)GetModule(typeof(T));

        public static T GetOrCreateModule<T> (CreateParameters parameters) where T:IModule
        {
            T module = GetModule<T>();
            if (module == null) CreateModule<T>(parameters);
            module = GetModule<T>();
            return module;
        }
        /// <summary>
        /// 销毁模块，如果模块不存在，则抛出异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void DestroyModule<T>() where T : IModule => DestroyModule(typeof(T));




        public static event Action Update = null;

        public static float deltaTime = 0;

        public static float timeScale = 1;
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
                    modules.Enqueue(key, module);
                }
            }
        }


    }

}

