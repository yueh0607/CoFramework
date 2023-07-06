# CoFramework   Guide

## 三、PoolModule

### 1. 引用池

非常简单，没有多余的操作，就是申请和回收，这是嵌入Core部分的

```c#
Framework.GlobalRecycle(object obj)
T Framework.GlobalAllocate<T>()
```

### 3. 对象池

也很简单

```c#

        //等初始化
		await Game.Instance.InitTask;
		//创建个池子
        var pool = Framework.GetModule<PoolModule>();
        pool.CreatePool("cube", "Cube", 10, 500);
		//创建父物体和列表
        List<GameObject> xs = new();
        GameObject Cubes = new GameObject();
		
		//生成1000个cube
        for (int i = 0; i < 1000; i++)
        {
            var x = await pool.Get("cube");
            xs.Add(x);
            x.transform.position = new Vector3(Random.value, Random.value, Random.value) * 100;
            x.transform.SetParent(Cubes.transform);
        }
		//一帧回收一个cube
        Debug.Log("回收开始");
        foreach (var t in xs)
        {
            await CoTask.NextFrame;
            pool.Set("cube", t);
        }
    }

```

