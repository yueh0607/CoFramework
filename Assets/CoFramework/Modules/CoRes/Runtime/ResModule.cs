using System;
using System.Collections;
using YooAsset;


namespace CoFramework.ResourceManagement
{

    //无依赖
    [ModuleDepends(typeof(ResModuleCreateParameters))]
    public class ResModule : IModule
    {

        ResourcePackage DefaultPackage = null;
        ResModuleCreateParameters _params;

        void IModule.OnCreate(CreateParameters parameters)
        {
            //参数检查
            _params = parameters as ResModuleCreateParameters;
            if (_params == null) throw new ArgumentException("Mismatched module parameter types");


            // 初始化资源系统
            YooAssets.Initialize();

            // 创建默认的资源包
            DefaultPackage = YooAssets.CreatePackage("DefaultPackage");

            // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
            YooAssets.SetDefaultPackage(DefaultPackage);
        }

        void IModule.OnDestroy()
        {

        }

        void IModule.OnUpdate()
        {

        }


        public AssetOperationHandle LoadAsync<T>(string location) where T : UnityEngine.Object
        {
            return YooAssets.LoadAssetAsync<T>(location);
        }


        public IEnumerator InitializeAsync()
        {
            switch (_params.Mode)
            {
                case EResourceMode.EditorSimulateMode:
                    {
                        var initParameters = new EditorSimulateModeParameters();
                        initParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(_params.DefaultPackageName);
                        yield return DefaultPackage.InitializeAsync(initParameters);
                        break;
                    }
                case EResourceMode.OfflinePlayMode:
                    {
                        var initParameters = new OfflinePlayModeParameters();
                        yield return DefaultPackage.InitializeAsync(initParameters);
                        break;
                    }
            }



        }



    }
}