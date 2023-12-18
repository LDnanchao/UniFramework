#if UNI_YOOASSET_SUPPORT
using System;
using UnityEngine;
using YooAsset;

namespace QFramework
{
    //需要关闭旧的资源加载方式
    public class UIKitWithYooAssetInit
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            UIKit.Config.PanelLoaderPool = new YooAssetPanelLoaderPool();
        }
    }
    public class YooAssetPanelLoaderPool : AbstractPanelLoaderPool
    {
        public class YooAssetPanelLoader : IPanelLoader
        {
            private GameObject mPanelPrefab;

            public GameObject LoadPanelPrefab(PanelSearchKeys panelSearchKeys)
            {
                if (panelSearchKeys.GameObjName.IsNotNullAndEmpty())
                {
                    mPanelPrefab = YooAssetKit.Package.LoadAssetSync<GameObject>(panelSearchKeys.GameObjName).AssetObject as GameObject;
                }
                else
                {
                    mPanelPrefab = YooAssetKit.Package.LoadAssetSync<GameObject>(panelSearchKeys.PanelType.Name).AssetObject as GameObject;
                }

                return mPanelPrefab;
            }

            public void LoadPanelPrefabAsync(PanelSearchKeys panelSearchKeys, Action<GameObject> onPanelLoad)
            {

                if (panelSearchKeys.GameObjName.IsNotNullAndEmpty())
                {
                    AssetHandle request = YooAssetKit.Package.LoadAssetAsync<GameObject>(panelSearchKeys.GameObjName);
                    request.Completed += operation => { onPanelLoad(request.AssetObject as GameObject); };
                }
                else
                {
                    AssetHandle request = YooAssetKit.Package.LoadAssetAsync<GameObject>(panelSearchKeys.PanelType.Name);
                    request.Completed += operation => { onPanelLoad(request.AssetObject as GameObject); };
                }

            }

            public void Unload()
            {
                mPanelPrefab = null;
            }
        }
        protected override IPanelLoader CreatePanelLoader()
        {
            return new YooAssetPanelLoader();
        }
    }
}
#endif