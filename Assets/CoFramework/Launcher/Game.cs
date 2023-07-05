using CoFramework.Events;
using CoFramework.Pool;
using CoFramework.ResourceManagement;
using CoFramework.Tasks;
using CoFramework;
using CoFramework.Utility.Singletons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoSingleton<Game>
{
    static bool inited = false;
    public CoTask InitTask
    {
        get
        {
            return CoTask.WaitUntil(() => inited);
        }
    }
    public bool Inited => inited;
    

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static async void Init()
    {
        SingletonInitializeAfterSceneLoad();

        Framework.CreateModule<TaskModule>(null);
        Framework.CreateModule<EventModule>(new EventModuleCreateParameters());
        Framework.CreateModule<ResModule>(new ResModuleCreateParameters() { });
        Framework.CreateModule<PoolModule>(null);

        var res = Framework.GetModule<ResModule>();
        await res.InitializeAsync();

        inited = true;
    }


    void Update()
    {
        Framework.DriveUpdate(Time.deltaTime);
    }
}
