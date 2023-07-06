using CoFramework;
using CoFramework.Events;
using CoFramework.Pool;
using CoFramework.ResourceManagement;
using CoFramework.Tasks;
using CoFramework.Tween;
using CoFramework.UI;
using CoFramework.Utility.Singletons;
using UnityEngine;

public class Game : MonoSingleton<Game>
{
    static bool inited = false;
    public static CoTask InitTask
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
        Framework.CreateModule<EventModule>(new EventModuleCreateParameters() { });
        Framework.CreateModule<ResModule>(new ResModuleCreateParameters() { });
        Framework.CreateModule<PoolModule>(null);
        Framework.CreateModule<TweenModule>(null);
        var res = Framework.GetModule<ResModule>();
        await res.InitializeAsync();
        Framework.CreateModule<UIModule>(new UIModuleCreateParameters() { });
        inited = true;
    }


    void Update()
    {
        Framework.DriveUpdate(Time.deltaTime);
    }
}
