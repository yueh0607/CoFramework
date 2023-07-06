# CoFramework   Guide

## 三、EventModule

1. 它能约束参数的类型
2. 他能约束调用类型
3. 它是全泛型实现

2).事件定义

定义事件有两种接口，ISendEvent是不带返回值的,ICallEvent是带返回值的

```csharp
//这一部分如果涉及多人开发，您可以把所有事件放在一个文件夹下，也可也放在事件的最相关处，这样做可以方便查阅

public interface MyEvent: ISendEvent<参数类型...>
public interface MyEvent: ICallEvent<参数类型...返回值类型>
```

3).注册和取消事件

仅在注册后才能执行发送和调用

``` csharp
this.Operator<消息类型接口>().Subcribe(MyAction);

//请注意，C#的委托闭包问题，也就是说如果您不取消事件，委托内被提取的引用对象将不会被GC自动回收，这是C#委托常见的一个内存泄漏陷阱。这是一个几乎全部事件系统都存在的问题。
this.Operator<消息类型接口>().UnSubcribe(MyAction);
```

4).调用和发送

``` csharp
//调用全部
this.Operator<消息类型接口>().Send(...参数们);
//调用全部
var results = this.Operator<消息类型接口>().Call(...参数们);
//调用第一个
var result = this.Operator<消息类型接口>().CallFirst(参数们);
```

5).例程

```csharp
using CoEvent;
using UnityEngine;

public interface IMyTest : ISendEvent<int, int> { }
public class Test : MonoBehaviour
{

    void Ttt(int t,int k)
    {
        Debug.Log($"{t}:{k}");
    }

    void Start()
    {
        this.Operator<IMyTest>().Subscribe(Ttt);
        this.Operator<IMyTest>().Send(10,100);
    }
    
    void OnDestroy()
    {
        this.Operator<IMyTest>().UnSubscribe(Ttt);
    }
}
```