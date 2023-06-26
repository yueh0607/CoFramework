using CoFramework;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
public class ResModule : IModule
{
    void IModule.OnCreate(CreateParameters parameters)
    {

    }

    void IModule.OnDestroy()
    {

    }

    void IModule.OnUpdate()
    {

    }


    public IEnumerator<AssetOperationHandle> LoadAsync(string location)
    {
        yield return YooAssets.LoadAssetAsync<Object>(location);
    }




}
