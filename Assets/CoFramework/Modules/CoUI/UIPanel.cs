using CoFramework.ResourceManagement;
using CoFramework.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CoFramework.UI
{
    public abstract class UIPanel
    {
        public GameObject Panel { get; set; }

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
            return OnOpen();
        }


        public CoTask Close()
        {
            if (!loaded) throw new InvalidOperationException("Panel has been not loaded.");
            if (unloading) throw new InvalidOperationException("Panel is unloading.");
            return  OnClose();
        }

        private Action updateAction =null;
        public async CoTask Load(string location)
        {
            if (loaded) throw new InvalidOperationException("Panel has been loaded.");
            if (unloading) throw new InvalidOperationException("Panel is unloading.");
            var res = Framework.GetModule<ResModule>();
            var handle = res.LoadAsync<GameObject>(location);
            await handle;
            var insHandle = handle.InstantiateAsync();
            Panel = insHandle.Result;
            handle.Release();
            await OnCreate();
            loaded = true;
            updateAction ??= Update;
            Framework.Update += updateAction;
        }

        public async CoTask Unload()
        {
            if (!loaded) throw new InvalidOperationException("Panel has been not loaded.");
            if (unloading) throw new InvalidOperationException("Panel is unloading.");
            unloading= true;
            Framework.Update -= updateAction;
            await OnDestroy();
            GameObject.Destroy(Panel);
            loaded = false;
        }



    }
}