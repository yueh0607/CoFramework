using CoFramework.Tasks.Internal;
using CoFramework;
using CoFramework.Events;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Collections;

namespace CoFramework.Tasks
{

    public abstract class CoTaskBase : CoObject, IAsyncTokenProperty
    {




        #region TaskBase
        /// <summary>
        /// 异步任务现在执行的状态
        /// </summary>
        public ETaskStatus Status { get; protected set; } = ETaskStatus.None;

        /// <summary>
        /// 任务是否已经完成，包含失败和成功
        /// </summary>
        public bool IsDone
        {
            get
            {
                if (Status == ETaskStatus.Succeed || Status == ETaskStatus.Failed) return true;
                return false;
            }
        }



        private Action finish = null;


        /// <summary>
        /// 令任务成功完成
        /// </summary>
        public Action FinishSucceed
        {
            get
            {
                finish ??= () => { Finish(ETaskStatus.Succeed); };
                return finish;
            }
        }

        /// <summary>
        /// 以某个状态完成任务
        /// </summary>
        /// <param name="status"></param>
        public void Finish(ETaskStatus status)
        {
            if (IsDone) return;
            Status = status;
            if (!Token.IsCanceld)
                OnFinish();
            OnRecycle();
        }


        /// <summary>
        /// 在任务完成时调用以推动状态机
        /// </summary>
        protected abstract void OnFinish();
        /// <summary>
        /// 在任务结束后调用以回收或释放任务
        /// </summary>
        protected abstract void OnRecycle();

        /// <summary>
        /// 推送异常，在出现异常时调用，异常将按指定规则处理
        /// </summary>
        /// <param name="ex"></param>
        public void PostException(Exception ex)
        {

        }

        public CoTaskBase()
        {
            Token = new AsyncTreeTokenNode(this, this);
        }
        #endregion

        #region TokenTree


        /// <summary>
        /// 异步令牌，用于控制任务能否完成和执行
        /// </summary>
        public AsyncTreeTokenNode Token { get; set; } = null;


        #endregion




        #region Pool
        /// <summary>
        /// 对TaskBase的初始化
        /// </summary>
        protected void BaseAllocate()
        {
            Status = ETaskStatus.None;

            var token = ((IAsyncTokenProperty)this).Token;
            token.Current = this;
            token.Root = this;
            token.Authorization = true;
            token.IsCanceld = false;
            
        }

        /// <summary>
        /// 对TaskBase的终结或回收
        /// </summary>
        protected void BaseRecycle()
        {
            var token = ((IAsyncTokenProperty)this).Token;
            token.Authorization = false;
        }


        #endregion
    }
}