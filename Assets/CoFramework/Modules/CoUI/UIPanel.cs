using CoFramework.ResourceManagement;
using CoFramework.Tasks;
using System;
using UnityEngine;

namespace CoFramework.UI
{
    public abstract class UIPanel
    {
        public GameObject Panel { get; set; }

        public CanvasGroup Group { get; set; }

        public bool Loaded => loaded;
        private bool loaded = false;
        private bool unloading = false;
        protected abstract CoTask OnOpen();

        protected abstract CoTask OnClose();

        protected abstract CoTask OnCreate();

        protected abstract CoTask OnDestroy();

        protected abstract void Update();

        public CoTask Open()
        {
            if (!loaded) throw new InvalidOperationException("Panel has been not loaded.");
            if (unloading) throw new InvalidOperationException("Panel is unloading.");
            Group.alpha = 1;
            return OnOpen();
        }


        public CoTask Close()
        {
            if (!loaded) throw new InvalidOperationException("Panel has been not loaded.");
            if (unloading) throw new InvalidOperationException("Panel is unloading.");
            Group.alpha = 0;
            return OnClose();
        }

        private Action updateAction = null;
        public async CoTask Load(string location)
        {
            //条件检查
            if (loaded) throw new InvalidOperationException("Panel has been loaded.");
            if (unloading) throw new InvalidOperationException("Panel is unloading.");
            //面板加载
            var res = Framework.GetModule<ResModule>();
            var handle = res.LoadAsync<GameObject>(location);
            await handle;
            var insHandle = handle.InstantiateAsync();
            await insHandle;
            Panel = insHandle.Result;
            Group = Panel.GetComponent<CanvasGroup>();
            if (Group == null) throw new ArgumentException("Panel must have canvas group");
            //面板处理
            GameObject.DontDestroyOnLoad(Panel);
            var module = Framework.GetModule<UIModule>();
            Panel.transform.SetParent(module.UIRoot.transform,true);
 

            //生命周期
            handle.Release();
            await OnCreate();
            loaded = true;
            Debug.Log("loaded");
            updateAction ??= Update;
            Framework.Update += updateAction;
        }

        public async CoTask Unload()
        {
            if (!loaded) throw new InvalidOperationException("Panel has been not loaded.");
            if (unloading) throw new InvalidOperationException("Panel is unloading.");
            unloading = true;
            Framework.Update -= updateAction;
            await OnDestroy();
            GameObject.Destroy(Panel);
            Panel= null;
            Group= null;
            loaded = false;
        }



    }
}