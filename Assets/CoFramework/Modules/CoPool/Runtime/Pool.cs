﻿using CoFramework.ResourceManagement;
using CoFramework.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace CoFramework.Pool
{

    public class Pool
    {

        public Action<GameObject> OnGet { get; set; } = null;
        public Action<GameObject> OnSet { get; set; } = null;
        public Action<GameObject> OnDestroy { get; set; } = null;

        public Action<GameObject> OnCreate { get; set; } = null;

        public int MaxCount { get; set; } = int.MaxValue;

        public int MinCount { get; set; } = 0;

        public string Location { get; set; } = null;



        private AssetOperationHandle cacheHandle;

        private Queue<GameObject> queue = new Queue<GameObject>();


        internal bool _destroyed = false;
        private IEnumerator PoolThread()
        {
            var module = Framework.GetModule<ResModule>();
            cacheHandle = module.LoadAsync<GameObject>(Location);
            yield return cacheHandle;
            while (true)
            {
                if(_destroyed)
                {
                    cacheHandle.Release();
                    yield break;
                }
                if (queue.Count < MinCount)
                {
                    var handle = cacheHandle.InstantiateAsync();
                    yield return handle;
                    OnCreate?.Invoke(handle.Result);
                    queue.Enqueue(handle.Result);
                }
                yield return null;
            }
            
        }
        internal void InitPool()
        {
            var module = Framework.GetModule<TaskModule>();
            module.Mono.StartCoroutine(PoolThread());
        }


        public async CoTask<GameObject> Get()
        {
            if (queue.Count == 0)
            {
                var handle = cacheHandle.InstantiateAsync();
                await handle;
                OnCreate?.Invoke(handle.Result);
                OnGet?.Invoke(handle.Result);
                return handle.Result;
            }
            await CoTask.CompletedTask;
            return queue.Dequeue();
        }

        public void Set(GameObject item)
        {
            if (queue.Count >= MaxCount)
            {
                OnDestroy?.Invoke(item);
                return;
            }
            else
            {
                OnSet?.Invoke(item);
                queue.Enqueue(item);
            }
        }
    }
}