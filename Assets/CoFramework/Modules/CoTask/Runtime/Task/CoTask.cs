using CoFramework.Events;
using CoFramework.Tasks.Internal;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.SocialPlatforms;

namespace CoFramework.Tasks
{
    [AsyncMethodBuilder(typeof(CoTaskBuilder))]
    public partial class CoTask : CoTaskBase, IAsyncTask, IAsyncTokenProperty
    {
        /// <summary>
        /// 状态切换
        /// </summary>
        protected Action continuation = null;

        /// <summary>
        /// 任务是否已经完成，与IsDone等同
        /// </summary>
        public bool IsCompleted => base.IsDone;

        /// <summary>
        /// 创建并返回一个CoTask
        /// </summary>
        /// <returns></returns>
        public static CoTask Create()
        {
            CoTask task = Framework.GlobalAllocate<CoTask>();
            task.BaseAllocate();
            return task;
        }

        /// <summary>
        /// 编编译器调用警告：本方法由编译器生成到用户调用以返回异步结果
        /// </summary>
        public void GetResult() { }

        /// <summary>
        /// 在任务完成时被调用
        /// </summary>
        public event Func<CoTask> OnAwait = null;

        /// <summary>
        /// 编编译器调用警告：本方法由编译器调用以支持await关键字
        /// </summary>
        public CoTask GetAwaiter()
        {
            if (OnAwait == null) return this;
            var task = OnAwait();
            task.OnCompleted += FinishWith;
            return task;
        }

        void INotifyCompletion.OnCompleted(Action continuation) => ((ICriticalNotifyCompletion)this).UnsafeOnCompleted(continuation);
        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation)
        {
            this.continuation = continuation;
        }

        private Action<CoTask> _callback = null;

        /// <summary>
        /// 在任务完成时被调用
        /// </summary>
        public event Action<CoTask> OnCompleted
        {
            add
            {
                if (!base.IsDone)
                {
                    _callback += value;
                }
                else
                {
                    _callback += value;
                    value?.Invoke(this);
                }
            }
            remove
            {
                _callback -= value;
            }
        }

        private Action<CoTask> finishWith = null;
        public Action<CoTask> FinishWith
        {
            get
            {
                finishWith ??= (x) => Finish(x.Status);
                return finishWith;
            }
        }


        protected override void OnFinish()
        {

            _callback?.Invoke(this);
            continuation?.Invoke();
        }


        protected override void OnRecycle()
        {
            _callback = null;
            base.BaseRecycle();
            continuation = null;
        }



    }


    [AsyncMethodBuilder(typeof(CoTaskBuilder<>))]

    public partial class CoTask<T> : CoTask, IAsyncTask<T>, IAsyncTokenProperty
    {

        public T Result { get; set; }

        private Action<T> finsh_withResult = null;
        public Action<T> FinishSucceedWithResult
        {
            get
            {
                finsh_withResult ??= (x) =>
                {
                    Result = x;
                    Finish(ETaskStatus.Succeed);
                };
                return finsh_withResult;
            }
        }

        public static new CoTask<T> Create()
        {
            CoTask<T> task = Framework.GlobalAllocate<CoTask<T>>();
            task.BaseAllocate();
            return task;
        }

        public new T GetResult() => Result;


        /// <summary>
        /// 在任务完成时被调用
        /// </summary>
        public new event Func<CoTask<T>> OnAwait = null;

        public new CoTask<T> GetAwaiter()
        {
            if (OnAwait == null) return this;
            var task = OnAwait();
            task.OnCompleted += FinishWith;
            return task;
        }

        private Action<CoTask<T>> _callback = null; 
        /// <summary>
        /// 在任务完成时被调用
        /// </summary>
        public new event Action<CoTask<T>> OnCompleted
        {
            add
            {
                if (!IsDone)
                {
                    _callback += value;
                }
                else
                {
                    _callback += value;
                    value?.Invoke(this);
                }
            }
            remove
            {
                _callback -= value;
            }
        }

        private Action<CoTask<T>> finishWith = null;
        public new Action<CoTask<T>> FinishWith
        {
            get
            {
                finishWith ??= (x) => Finish(x.Status);
                return finishWith;
            }
        }
        protected override void OnRecycle()
        {
            base.OnRecycle();
            Result = default;
        }


    }





    public struct CoTaskCompleted : INotifyCompletion, ICoTask
    {
        public bool IsCompleted => true;
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetResult() { }
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetException(Exception exception) { }

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CoTaskCompleted GetAwaiter() => this;
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnCompleted(Action continuation) { }
    }
}
