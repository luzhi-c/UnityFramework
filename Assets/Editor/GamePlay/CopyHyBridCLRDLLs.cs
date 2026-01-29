using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor
{
    public class CopyHyBridCLRDLLs : UnityEditor.Editor
    {
        // 直接在代码里修改这些常量
        private static string[] HOTFIX_FILE_NAMES = { "GamePlay.Game.dll" };  // 要复制的文件名
        private static string[] AOT_FILE_NAMES = { "mscorlib.dll", "YooAsset.dll", "System.Core.dll", "GameFramework.dll" };  // 要复制的文件名
        private static string HotFixSourceFolder = "HybridCLRData/HotUpdateDlls";  // 相对于项目根目录
        private static string AOTSourceFolder = "HybridCLRData/AssembliesPostIl2CppStrip";  // 相对于项目根目录
        private static string HotFixtargetFolder = "HotUpdateDlls";  // 目标Unity路径
        private static string AOTtargetFolder = "AOTDlls";  // 目标Unity路径

        [MenuItem("Tools/HybridCLR/复制热更DLL")]
        public static void CopyHotUpdateDLLs()
        {
            string projectRoot = Application.dataPath.Replace("/Assets", "");
            // 获取项目根目录
            // 解析源文件夹路径（支持相对路径）
            string sourcePath = Path.GetFullPath(Path.Combine(projectRoot, HotFixSourceFolder));
            string targetPath = Path.GetFullPath(Path.Combine(Application.dataPath, HotFixtargetFolder));
            CopyFiles(sourcePath, targetPath, HOTFIX_FILE_NAMES);
        }

        [MenuItem("Tools/HybridCLR/复制AOTDLL")]
        public static void CopyAOTDLLs()
        {
            string projectRoot = Application.dataPath.Replace("/Assets", "");
            // 获取项目根目录
            // 解析源文件夹路径（支持相对路径）
            string sourcePath = Path.GetFullPath(Path.Combine(projectRoot, AOTSourceFolder));
            string targetPath = Path.GetFullPath(Path.Combine(Application.dataPath, AOTtargetFolder));
            CopyFiles(sourcePath, targetPath, AOT_FILE_NAMES);
        }

        static void CopyFiles(string sourcePath, string targetPath, string[] files)
        {
            var activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            if (activeBuildTarget == BuildTarget.StandaloneWindows64)
            {
                sourcePath = Path.GetFullPath(Path.Combine(sourcePath, "StandaloneWindows64"));
            }
            else if (activeBuildTarget == BuildTarget.Android)
            {
                sourcePath = Path.GetFullPath(Path.Combine(sourcePath, "Android"));
            }
            Debug.Log($"源目录: {sourcePath}");
            Debug.Log($"目标目录: {targetPath}");
            if (!Directory.Exists(sourcePath))
            {
                Debug.LogError($"源目录不存在: {sourcePath}");
                return;
            }

            // 确保目标目录存在
            if (!Directory.Exists(targetPath))
            {
                Debug.LogError($"目标目录不存在: {sourcePath}");
                return;
            }
            try
            {
                for (int i = 0; i < files.Length; i++)
                {
                    string fileName = Path.Combine(sourcePath, files[i]);
                    string targetFile = Path.Combine(targetPath, files[i] + ".bytes");

                    File.Copy(fileName, targetFile, true);
                    Debug.Log($"已复制: {fileName} → {Path.GetFileName(targetFile)}");
                }

                // 刷新Asset数据库
                AssetDatabase.Refresh();

            }
            catch (System.Exception e)
            {
                Debug.LogError($"复制失败: {e.Message}");
            }
        }
    }
}

