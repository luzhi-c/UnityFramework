using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.ProjectWindowCallback;
using Unity.VisualScripting;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer;

public class AutoUICreator
{
    // 脚本
    private const string UI_SCRIPT_TEMPLATE = "Assets/Editor/UITemplates/AutoUIBindingTemplate.cs.txt";

    [MenuItem("Assets/Create/AutoUIBinding", false, 80)]
    public static void AutoUIBinding()
    {
        string locationPath = GetSelectedPathOrFallback();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
            0,
        ScriptableObject.CreateInstance<MyDoCreateScriptAsset>(),
        locationPath + "/MyNewBehaviorScript.cs", null, UI_SCRIPT_TEMPLATE
        );
    }

    static string GetSelectedPathOrFallback()
    {
        string path = "Assets";
        foreach (var obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }

    class MyDoCreateScriptAsset : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            UnityEngine.Object o = CreateScriptAssetByTemplate(pathName, resourceFile);
            ProjectWindowUtil.ShowCreatedAsset(o);
        }

        static UnityEngine.Object CreateScriptAssetByTemplate(string pathName, string resourceFile)
        {
            string fullPath = Path.GetFullPath(pathName);
            string text = "";
            using (StreamReader streamReader = new StreamReader(resourceFile))
            {
                text = streamReader.ReadToEnd();
                streamReader.Close();
                streamReader.Dispose();
            }

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathName);
            text = Regex.Replace(text, "#Name#", fileNameWithoutExtension);
            bool encoderShouldEmitUTF8Identifier = true;
            bool throwInvalidBytes = false;
            UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwInvalidBytes);
            bool append = false;
            using (StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding))
            {
                streamWriter.Write(text);
                streamWriter.Close();
                streamWriter.Dispose();

                AssetDatabase.ImportAsset(pathName);
            }
            return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
        }
    }
}
