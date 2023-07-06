# CoFramework   Guide

## 三、TaskModule

TaskMoudle作为代替链式任务的核心模块，驱动着大半个框架的异步运行，他的性能虽然远不如UniTask与ETTask，但是他的便捷程度却不会低。

1. 它支持取消的令牌的自动传递，暂停，继续

2. 它支持对协程的await取消，暂停，继续，能几乎无缝对接unity协程
3. 对大部分工具不需要对接过程，直接await即可，如不需要适配YooAsset

凭这三点，已经令该Task模块的便捷程度上一层楼，无需进行令牌传染即可完成取消暂停等动作，无需配置。



下面是简单的使用，注意在TaskModule中不止下面的内容，您可以直接通过CoTask.XX来进行调用，其余均作为拓展方法使用。

```C#
//可等待的任务
public async CoTask mTest()
{
	//等待600帧
    await CoTask.WaitForFrame(600);
    Debug.Log("Hha");
    //等待3秒
    await CoTask.Delay(3);
    Debug.Log("111");
}

```

```c#
//以下两种没有任何意义，仅用于消除返回值的警告
mTest().Forget();
_ = mTest();

//如果您获取到了一个没有在执行的CoTask,可以这样开启任务
task.Invoke();
```

```c#

//取得令牌
mTest().WithToken(out var token);
//暂停
token.Yield();
//继续
token.Continue();

//取消,如果您担心GC问题，可以实现接口来代替此方案
token.OnCanceled +=()=>
{
    Debug.Log("任务取消后的处理");
}
//取消任务
token.Cancel();
```

```csharp
//延迟一秒
CoTask.Delay(1);
//转协程
CoTask.ToCoroutine(async ()=>
{
	await CoTask.Delay(1);
})
//已经完成的Task
CoTask.CompletedTask

//获取未执行CpTask
Func<CoTask> task = async ()=>
{
	//这个lambda可以是异步方法，这样就获取了这个CoTask
}

```