#if HYBRIDCLR_SUPPORT

using System.Collections.Generic;
using System.IO;
using HybridCLR.Editor;
using HybridCLR.Editor.Settings;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using HybridCLRKit;

namespace HybridCLRKit.Editor
{
    public static class EditorTool
    {
        [MenuItem("HybridCLR/CopyToBundles", false)]
        public static void DllCopyToAsset()
        {
            var hotfixSetting =
                JsonUtility.FromJson<HotfixSetting>(AssetDatabase
                    .LoadAssetAtPath<TextAsset>("Assets/Config/HotfixSetting.json").text);
            // "D:\workplace\unity\GameFrameworkDemo\HybridCLRData\HotUpdateDlls\StandaloneWindows64";
            // "D:\workplace\unity\GameFrameworkDemo\Assets\AssetBundles\Dlls";
            string hotUpdateDllsPath = "HybridCLRData/HotUpdateDlls/Android";
            string toDir = "Assets/Dll";

            if (Directory.Exists(toDir))
            {
                Directory.Delete(toDir, true);
            }

            Directory.CreateDirectory(toDir);

            if (Directory.Exists(hotUpdateDllsPath))
            {
                foreach (var dllName in hotfixSetting.hotfixDlls)
                {
                    File.Copy(Path.Combine(hotUpdateDllsPath, $"{dllName}.dll"),
                        Path.Combine(toDir, $"{dllName}.dll.bytes"), true);
                }
            }


            string aotDllsPath = "HybridCLRData/AssembliesPostIl2CppStrip/Android";
            Directory.CreateDirectory(toDir);

            if (Directory.Exists(aotDllsPath))
            {
                foreach (var dllName in hotfixSetting.aotDlls)
                {
                    File.Copy(Path.Combine(aotDllsPath, $"{dllName}.dll"), Path.Combine(toDir, $"{dllName}.dll.bytes"),
                        true);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        [MenuItem("HybridCLR/刷新设置", false)]
        public static void UpdateHybridSetting()
        {
            // HybridCLRSettings.Instance.hotUpdateAssemblyDefinitions = 
            // HybridCLRSettings.Instance.patchAOTAssemblies 
            var hotfixSetting =
                JsonUtility.FromJson<HotfixSetting>(AssetDatabase
                    .LoadAssetAtPath<TextAsset>("Assets/Config/HotfixSetting.json").text);

            //更新程序集
            List<AssemblyDefinitionAsset> hotUpdateAssemblyDefinitions = new();
            foreach (var hotfixDll in hotfixSetting.hotfixDlls)
            {
                string guid = AssetDatabase.FindAssets($"{hotfixDll} t: assemblyDefinitionAsset")[0];
                string path = AssetDatabase.GUIDToAssetPath(guid);
                hotUpdateAssemblyDefinitions.Add(AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(path));
            }
            HybridCLRSettings.Instance.hotUpdateAssemblyDefinitions = hotUpdateAssemblyDefinitions.ToArray();
            
            
            //HybridCLRSettings.Instance.hotUpdateAssemblyDefinitions 
            //补充Aot
            List<string> patchAOTAssembles = new List<string>();
            foreach (var dllName in hotfixSetting.aotDlls)
            {
                patchAOTAssembles.Add($"{dllName}.dll");
            }

            HybridCLRSettings.Instance.patchAOTAssemblies = patchAOTAssembles.ToArray();
            HybridCLRSettings.Save();
        }
    }
}
#endif