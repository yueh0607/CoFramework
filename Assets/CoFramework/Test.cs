using CoFramework;
using CoFramework.Tasks;
using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    object x = new();

 

    async CoTask AsyncTest()
    {
       CoTask<AsyncMonitor.LockHandle> task = AsyncMonitor.Lock(x);

        AsyncMonitor.LockHandle t = await task;
        if(task.Result==null)
        {
            int d = 0;
        }

        using (t)
        {
            await CoTask.Delay(3);
            Debug.Log("执行");
        }
        Debug.Log("Dispose");
       
  
    }


    async CoTask<int> TestAsync()
    {
        await CoTask.CompletedTask;
        return 10;
    }    

    async void Start()
    {
        Framework.CreateModule<TaskModule>(null);

        AsyncTest().Forget();
        AsyncTest().Forget();


       // Debug.Log(await TestAsync());
        //AsyncMonitor.Enter(x);

        //Debug.Log(AsyncMonitor.IsLocked(x));

    }


    // Update is called once per frame
    void Update()
    {
        Framework.DriveUpdate(Time.deltaTime);
    }
}
