using CoFramework;
using CoFramework.Pool;
using CoFramework.RefBuild;
using CoFramework.ResourceManagement;
using CoFramework.Tasks;
using CoFramework.Tween;
using System.Collections.Generic;
using UnityEngine;

/*模块依赖
 * 1.Framework
 * 2.Res / Task
 * 3.Event / Pool
 * 
 * 
 * 
 * 
 */


public class Test : MonoBehaviour
{


    BindableProperty<string> str = new("Hello");

    async void Start()
    {

        //Framework.CreateModule<TaskModule>(null);
        //Framework.CreateModule<EventModule>(new EventModuleCreateParameters());
        //Framework.CreateModule<ResModule>(new ResModuleCreateParameters() { });
        //Framework.CreateModule<PoolModule>(null);
        var tween = str.To("0HelloWorld").SetDuration(5);
        tween.Play();
        str.OnPropertyChanged += (x, y) =>
        {
            Debug.Log(y);
        };

        return;
        await Game.Instance.InitTask;

        var res = Framework.GetModule<ResModule>();
        //await res.InitializeAsync();
        var pool = Framework.GetModule<PoolModule>();


        pool.CreatePool("cube", "Cube", 10, 500);

        List<GameObject> xs = new();
        GameObject Cubes = new GameObject();


        for (int i = 0; i < 1000; i++)
        {
            var x = await pool.Get("cube");
            xs.Add(x);
            x.transform.position = new Vector3(Random.value, Random.value, Random.value) * 100;
            x.transform.SetParent(Cubes.transform);
        }
        //await CoTask.Delay(5);




        Debug.Log("回收开始");
        foreach (var t in xs)
        {
            await CoTask.NextFrame;
            pool.Set("cube", t);
        }

        Debug.Log("重启");
        for (int i = 0; i < 1000; i++)
        {
            var x = await pool.Get("cube");
            xs[i] = x;
            x.transform.position = new Vector3(Random.value, Random.value, Random.value) * 100;
            x.transform.SetParent(Cubes.transform);
        }
    }



}
