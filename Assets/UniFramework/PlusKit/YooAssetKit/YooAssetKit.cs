
#if UNI_YOOASSET_SUPPORT
using Cysharp.Threading.Tasks;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace QFramework
{
    public class YooAssetKit
    {
        public static async UniTask Init()
        {
            await YooAssetManager.Instance.Init();
        }
        public static ResourcePackage Package
        {
            get => YooAssetManager.Instance.Package;
        }
    }

}
#endif