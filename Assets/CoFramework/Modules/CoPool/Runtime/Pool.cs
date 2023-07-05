using CoFramework.ResourceManagement;
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

        public Action<GameObject> OnGet { get; set; } = defaultGet;
        public Action<GameObject> OnSet { get; set; } = defaultSet;
        public Action<GameObject> OnDestroy { get; set; } = defaultDestroy;

        public Action<GameObject> OnCreate { get; set; } = null;

        public int MaxCount { get; set; } = int.MaxValue;

        public int MinCount { get; set; } = 0;

        public string Location { get; set; } = null;



        private AssetOperationHandle cacheHandle;

        private Queue<GameObject> queue = new Queue<GameObject>();


        static readonly Action<GameObject> defaultGet = (x) =>
            {
                x.SetActive(true);
            }
        , defaultSet = (x) =>
        {
            x.SetActive(false);
        }
        , defaultDestroy = (x) =>
        {
            GameObject.Destroy(x);
        };



        internal bool _destroyed = false;
        private IEnumerator PoolThread()
        {
            var module = Framework.GetModule<ResModule>();
            cacheHandle = module.LoadAsync<GameObject>(Location);
            yield return cacheHandle;
            while (true)
            {
                if (_destroyed)
                {
                    cacheHandle.Release();
                    yield break;
                }
                if (MinCount > MaxCount) throw new InvalidOperationException($"Please keep MaxCount>MinCount . CurrentMin{MinCount}>CurreentMax{MaxCount}");
                if (queue.Count < MinCount)
                {
                    var handle = cacheHandle.InstantiateAsync();
                    yield return handle;
                    OnCreate?.Invoke(handle.Result);
                    queue.Enqueue(handle.Result);
                }
                if (queue.Count > MaxCount)
                {
                    OnDestroy?.Invoke(queue.Dequeue());
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
            var item = queue.Dequeue();
            OnGet?.Invoke(item);
            return item;
        }
        public GameObject GetSync()
        {
            if (queue.Count == 0)
            {
                var handle = cacheHandle.InstantiateSync();
                OnCreate?.Invoke(handle);
                OnGet?.Invoke(handle);
                return handle;
            }
            var item = queue.Dequeue();
            OnGet?.Invoke(item);
            return item;
        }

        public void Set(GameObject item)
        {
            if (queue.Count >= MaxCount)
            {
                OnSet?.Invoke(item);
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