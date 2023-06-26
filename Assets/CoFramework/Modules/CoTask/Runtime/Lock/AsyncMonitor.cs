using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoFramework.Tasks
{



    public static class AsyncMonitor
    {

        private class AsyncLockComparer : IEqualityComparer<object>
        {
            public new bool Equals(object x, object y)
            {
                return System.Object.ReferenceEquals(x, y);
            }

            public int GetHashCode(object obj)
            {
                return obj.GetHashCode();
            }
        }
        private static readonly HashSet<object> _locked = new HashSet<object>(new AsyncLockComparer());


        public static void Enter(object target)
        {
            if(_locked.Contains(target))  throw new InvalidOperationException("Repeated locking of an object is not allowed");
            _locked.Add(target);
        }
        public static void TryEnter(object target)
        {
            if (!_locked.Contains(target)) 
            _locked.Add(target);
        }


        public static void Exit(object target)
        {
            if (_locked.Contains(target)) throw new InvalidOperationException("Unlocking unlocked objects is not allowed");
            _locked.Remove(target);
        }

        public static void TryExit(object target)
        {
            if (_locked.Contains(target))
                _locked.Remove(target);
        }

        public static bool IsLocked(object target)
        {
            return _locked.Contains(target);
        }

        public static  CoTask WaitUnlocked(object obj)
        {
            var task = CoTask.Create();
            if (!AsyncMonitor.IsLocked(obj))
            {
                task.Finish(ETaskStatus.Succeed);
                return task;
            }
            static IEnumerator WaitUnlockedTask(object obj,CoTask task)
            {
                while (true)
                {
                    if(task.Token.IsCanceld)
                    {
                        task.Finish(ETaskStatus.Failed);
                        yield break;
                    }
                    if (!task.Token.Authorization) yield return null;
                    if (AsyncMonitor.IsLocked(obj))
                    {
                        Debug.Log($"Waiting{task.ID}:{AsyncMonitor.IsLocked(obj)}");
                        yield return null;
                        
                    }
                    else yield break;
                }
            }
           
            var module = Framework.GetModule<TaskModule>();
            module.Mono.StartCoroutine(WaitUnlockedTask(obj, task));
            return task;
        }

        public static async CoTask<LockHandle> Lock(object obj)
        {
            var handle = Framework.GlobalAllocate<LockHandle>();
            await handle.Lock(obj);
            if(handle==null)
            {
                int x = 0;
            }
            return handle;
        }

        public class LockHandle : IDisposable
        {
            private object target = null;
            public async CoTask Lock(object target)
            {
                this.target = target ?? throw new InvalidOperationException("Null cannot be locked");
                await WaitUnlocked(target);
                AsyncMonitor.TryEnter(target);
                //Debug.Log(IsLocked(target));
            }

            void IDisposable.Dispose()
            {
                //Debug.Log(target);
                AsyncMonitor.TryExit(target);
                Debug.Log(IsLocked(target));
                target = null;
                Framework.GlobalRecycle(this);
            }
        }


    }
}
