#if UNI_YOOASSET_SUPPORT
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;
using YooAsset;
using Cysharp.Threading.Tasks;

namespace QFramework
{
    public enum AssetLoadType
    {
        EditorSimulateMode,
        OfflinePlayMode,
        HostPlayMode
    }
    public class YooAssetManager:MonoSingleton<YooAssetManager>
    {
        private static ResourcePackage m_package = null;
        public bool IsInit = false;
        public AssetLoadType editorLoadType = AssetLoadType.EditorSimulateMode;
        public AssetLoadType runtimeLoadType = AssetLoadType.OfflinePlayMode;

        private YooAssetManager() { }
        public override  void OnSingletonInit()
        {
            base.OnSingletonInit();
           // await Init();

        }
        public ResourcePackage Package
        {
            get {
                return m_package;
            }
        }
        public async UniTask Init()
        {
            // 初始化资源系统
            YooAssets.Initialize();
            // 创建默认的资源包
            m_package = YooAssets.CreatePackage("DefaultPackage");

            // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
            YooAssets.SetDefaultPackage(m_package);
            
#if UNITY_EDITOR
            AssetLoadType loadType = editorLoadType;
#else
            AssetLoadType loadType = runtimeLoadType;
#endif
            switch (loadType)
            {
                case AssetLoadType.EditorSimulateMode:
                    await SetEditorSimulateMode();
                    break;
                case AssetLoadType.OfflinePlayMode:
                    await SetOfflinePlayMode();
                    break;
                case AssetLoadType.HostPlayMode:
                    break;
            }

            IsInit = true;
        }

        private async UniTask SetEditorSimulateMode()
        {
            var initParameters = new EditorSimulateModeParameters();
            initParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild("BuiltinBuildPipeline", "DefaultPackage");
            //initParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, "DefaultPackage");
            await m_package.InitializeAsync(initParameters).ToUniTask();
        }

        private async UniTask SetOfflinePlayMode()
        {
            var initParameters = new OfflinePlayModeParameters();
            await m_package.InitializeAsync(initParameters).ToUniTask();
        }

        public static T LoadAssetSync<T>(string path) where T : UnityEngine.Object
        {
            var handle = Instance.Package.LoadAssetSync<T>(path);
            T assetObject = handle.GetAssetObject<T>();
            if (assetObject == null)
            {
                throw new UnityException($"{typeof(T).FullName}:{path}加载失败！");
            }
            else
            {
                return assetObject;
            }
        }
    }
}
#endif