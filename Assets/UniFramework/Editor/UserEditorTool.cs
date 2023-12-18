using UnityEditor;
using UnityEngine;

namespace UserEditor
{
    public class UserEditorTool
    {
        [MenuItem("tools/删除玩家会话数据")]
        public static void DeletePlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("tools/Open PlayerPrefs Folder")]
        public static void OpenFolder()
        {
            string path = Application.persistentDataPath;
            path = path.Replace(@"/", @"\");
            System.Diagnostics.Process.Start("explorer.exe", "/select," + path);
        }
        

    }
    public class CEditor : Editor
    {

        [MenuItem("GameObject/CopyHierarchyPath", false, 11)]
        public static void CopyHierarchy()
        {
            GameObject go = Selection.activeGameObject;
            string path = go.GetRPath();
            path = path.Replace("Canvas (Environment)/", "");
            GUIUtility.systemCopyBuffer = path;
        }
       
    }

    public static class Ex
    {
        public static string GetRPath(this GameObject go)
        {
            GameObject current = go;
            string path = current.name;

            while (null != current.transform.parent)
            {
                current = current.transform.parent.gameObject;
                path = current.name + "/" + path;
            }

            return path;
        }
    }
}