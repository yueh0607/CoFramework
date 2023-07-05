using CoFramework.ResourceManagement;
using CoFramework.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace CoFramework.UI
{
    [ModuleDepends(typeof(UIModuleCreateParameters), typeof(ResModule), typeof(TaskModule))]
    public class UIModule : IModule
    {
        public GameObject UIRoot { get; private set; }
        public Camera UICamera { get; set; } = null;

        UIModuleCreateParameters _params = null;
        public void OnCreate(CreateParameters parameters)
        {
            //参数检查
            _params = parameters as UIModuleCreateParameters;
            if (_params == null) throw new ArgumentException("Mismatched module parameter types");

        }

        public void OnDestroy()
        {

        }

        public void OnUpdate()
        {

        }




        private Dictionary<Type, UIPanel> panels = new Dictionary<Type, UIPanel>();
        private readonly object GetWindowLocker = new object();
        public async CoTask<T> GetWindow<T>(bool single = true) where T : UIPanel
        {
            await Initialize();
            using (await AsyncMonitor.Lock(GetWindowLocker))
            {
                Type type = typeof(T);
                if (type.IsAbstract) throw new InvalidOperationException("Get Window don't accept abstract type");
                if (single)
                {
                    T p = Activator.CreateInstance<T>();
                    await p.Load(type.Name);
                    return p;
                }
                else
                {
                    if (!panels.ContainsKey(type))
                    {
                        T p = Activator.CreateInstance<T>();
                        panels.Add(type, p);
                        await p.Load(type.Name);
                    }
                    return (T)panels[type];
                }
            }
        }

        public async CoTask Open<T>() where T : UIPanel
        {
            await Initialize();
            Type type = typeof(T);
            T panel;
            if (!panels.ContainsKey(type))
            {
                panel = await GetWindow<T>();
            }
            else panel = (T)panels[type];
            await panel.Open();
        }
        public async CoTask Close<T>() where T : UIPanel
        {
            await Initialize();
            Type type = typeof(T);
            T panel;
            if (!panels.ContainsKey(type))
            {
                throw new InvalidOperationException("Please Close a exist panel");
            }
            else panel = (T)panels[type];
            await panel.Close();
        }


        private readonly object InitializeLocker = new object();
        private async CoTask Initialize()
        {
            if (AsyncMonitor.IsLocked(InitializeLocker))
            {
                await CoTask.CompletedTask; return;
            }
            else AsyncMonitor.Enter(InitializeLocker);

            var res = Framework.GetModule<ResModule>();

            //UIRoot
            var handle = res.LoadAsync<GameObject>(_params.UIRootLocation);
            await handle;
            var insHandle = handle.InstantiateAsync();
            await insHandle;
            UIRoot = insHandle.Result;

            //UICamera
            if (_params.NeedUICamera)
            {
                var handle2 = res.LoadAsync<GameObject>(_params.UICameraLocation);
                await handle2;
                var insHandle2 = handle.InstantiateAsync();
                await insHandle2;
                UICamera = insHandle2.Result.GetComponent<Camera>();

                Canvas canvas = UIRoot.GetComponent<Canvas>();
                if (canvas == null) throw new InvalidOperationException("UIRoot require Canvas Component");
                if (!canvas.isRootCanvas) throw new InvalidOperationException("UIRoot muse be rootCanvas");
                canvas.worldCamera = UICamera;
            }


            AsyncMonitor.Exit(InitializeLocker);
        }
    }
}