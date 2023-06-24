

//打开这行注释以支持DoTween完成的等待

#define CoEvent_Async_AsyncOperation_Enable

#if CoEvent_Async_AsyncOperation_Enable

using System;
using UnityEngine;

namespace CoFramework.Tasks
{

    public static class AsyncOperationEx
    {
        public static CoTask<AsyncOperation> GetAwaiter(this AsyncOperation operation)
        {
            var task = CoTask<AsyncOperation>.Create();
            operation.completed += task.FinishSucceedWithResult;
            return task;

        }
        public static AsyncOperation WithProgress(this AsyncOperation operation, Action<float> progress)
        {
            var module = Framework.GetModule<TaskModule>();
            Action callback =() =>
            {
                progress?.Invoke(operation.progress);
            };
            operation.completed += (x) =>
            {
                module.UpdateTask-=callback;
            };
            module.UpdateTask += callback;
            return operation;
        }



    }
}
#endif