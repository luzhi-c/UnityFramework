using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using GameFramework;
using System.Text;

[CustomEditor(typeof(GameObject))]
public class PrefabChildViewerEditor : Editor
{
    private const string UI_SCRIPT_OUTPUT = "Assets/GameMain/GamePlay/Scripts/_Auto/";
    private const string UI_SCRIPT_TEMPLATE = "Assets/Editor/UITemplates/AutoUIBindingTemplate.cs.txt";

    public override void OnInspectorGUI()
    {
        // 先绘制默认的Inspector内容
        // DrawDefaultInspector();

        // GameObject targetObject = (GameObject)target;

        // // 检查是否是Prefab实例或Prefab资源
        // bool isPrefabInstance = PrefabUtility.IsPartOfAnyPrefab(targetObject);
        // bool isPrefabAsset = PrefabUtility.IsPartOfPrefabAsset(targetObject);

        // if (isPrefabInstance || isPrefabAsset)
        // {
        //     EditorGUILayout.Space();
        //     EditorGUILayout.LabelField("Prefab Tools", EditorStyles.boldLabel);

        //     if (GUILayout.Button("遍历子节点"))
        //     {
        //         Generator(targetObject);
        //     }

        // }
    }

    void Generator(GameObject prefab)
    {

        string text = "";
        using (StreamReader streamReader = new StreamReader(UI_SCRIPT_TEMPLATE))
        {
            text = streamReader.ReadToEnd();
            streamReader.Close();
            streamReader.Dispose();
        }
        var fileName = Utility.Text.Format("{0}Mediator", prefab.name);
        string fullPath = Path.GetFullPath(Utility.Text.Format("{0}{1}.cs", UI_SCRIPT_OUTPUT, fileName));

        string directoryPath = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
        {
            Debug.Log($"创建目录: {directoryPath}");
            Directory.CreateDirectory(directoryPath);

            // 刷新AssetDatabase（如果是在Assets目录下）
            if (directoryPath.StartsWith("Assets/"))
            {
                AssetDatabase.Refresh();
            }
        }
        text = Regex.Replace(text, "#Name#", fileName);
        using (StreamWriter streamWriter = new StreamWriter(fullPath, false, new UTF8Encoding(true, false)))
        {
            streamWriter.Write(text);
            streamWriter.Close();
            streamWriter.Dispose();
        }
    }



}