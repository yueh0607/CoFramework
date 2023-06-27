﻿using CoFramework.Tasks;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CoFramework.Pool
{
    //依赖ResModule

    public class PoolModule : IModule
    {
        void IModule.OnCreate(CreateParameters parameters)
        {

        }

        void IModule.OnDestroy()
        {
            foreach(var kvp in pools)
            {
                kvp.Value._destroyed = true;
            }
            pools.Clear();
        }

        void IModule.OnUpdate()
        {

        }

        private readonly Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

        public void CreatePool(string key, string location, int minCount = 0, int maxCount = int.MaxValue,
            Action<GameObject> onCreate = null, Action<GameObject> onGet = null,
            Action<GameObject> onSet = null, Action<GameObject> onDestroy = null)
        {
            if (pools.ContainsKey(key)) return;
            Pool pool = new Pool();

            pool.Location = location;
            pool.MinCount = minCount;
            pool.MaxCount = maxCount;
            pool.OnCreate = onCreate;
            pool.OnGet = onGet;
            pool.OnSet = onSet;
            pool.OnDestroy = onDestroy;

            pools.Add(key,pool);

            pool.InitPool();
        }
        public Pool GetPool(string key)
        {
            if (!pools.ContainsKey(key)) return null;
            return pools[key];
        }

        public void DestroyPool(string key, bool exThrow = false)
        {
            if (!pools.ContainsKey(key))
            {
                if (exThrow)
                    throw new NullReferenceException("Destroying non-existent pool is not allowed");
                else return;
            }
            pools[key]._destroyed = true;
            pools.Remove(key);
        }


        public CoTask<GameObject> Get(string key)
        {
            var pool = GetPool(key)??throw new InvalidOperationException("Please get after create pool");
            
            return pool.Get();
        }

        public void Set(string key, GameObject obj)
        {
            var pool = GetPool(key) ?? throw new InvalidOperationException("Please set after create pool");
            pool.Set(obj);
        }

        public async CoTask<T> GetCom<T>(string key) where T:Component
        {
            var pool = GetPool(key);
            return (await pool.Get()).GetComponent<T>();
        }
        public void SetCom<T>(string key, T com) where T : Component
        {
            var pool = GetPool(key);
            pool.Set(com.gameObject);
        }
    }
}