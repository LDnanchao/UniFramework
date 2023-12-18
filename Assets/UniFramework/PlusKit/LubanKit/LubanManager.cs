using QFramework;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LubanManager : MonoSingleton<LubanManager>
{
    public cfg.Tables lubanTables = null;
    public override void OnSingletonInit()
    {
        base.OnSingletonInit();
        var tables = new cfg.Tables(LoadByteBufByYooAsset);
        lubanTables = tables;
    }
    
    private static JSONNode LoadByteBuf(string file)
    {
        return JSON.Parse(File.ReadAllText("Assets/Config/Json/" + file + ".json", System.Text.Encoding.UTF8));
    }
    private static JSONNode LoadByteBufByYooAsset(string file)
    {
        return JSON.Parse(YooAssetKit.Package.LoadAssetSync<TextAsset>(file).GetAssetObject<TextAsset>().text);
    }
}
