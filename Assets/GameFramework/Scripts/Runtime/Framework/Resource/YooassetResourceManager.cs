using System;
using System.Collections.Generic;
using GameFramework;
using YooAsset;
namespace UnityGameFramework.Runtime
{
    public class YooassetResourceManager : Singleton<YooassetResourceManager>
    {
        private Dictionary<string, ResourcePackage> m_Packages;
        private string m_DefaultPackageName = "DefaultPackage";

        public string DefaultPackageName
        {
            get => m_DefaultPackageName;
            set => m_DefaultPackageName = value;
        }
        private Dictionary<string, List<AssetHandleInfo>> m_AssetHandleMap;

        private Dictionary<string, SceneHandle> m_SceneHandle;
        public YooassetResourceManager()
        {
            m_Packages = new Dictionary<string, ResourcePackage>();
            m_AssetHandleMap = new Dictionary<string, List<AssetHandleInfo>>();
            m_SceneHandle = new Dictionary<string, SceneHandle>();
        }

        public void SetDefaultPackageName(string packageName)
        {
            m_DefaultPackageName = packageName;
        }

        public void AddPackage(string packageName, ResourcePackage package)
        {
            if (m_Packages.ContainsKey(packageName))
            {
                return;
            }
            m_Packages.Add(packageName, package);
        }

        public void RemovePackage(string packageName)
        {
            if (m_Packages.TryGetValue(packageName, out ResourcePackage package))
            {
                YooAssets.RemovePackage(package);
                m_Packages.Remove(packageName);
            }
        }

        public void LoadScene(string sceneName)
        {
            var sceneHandle = YooAssets.LoadSceneAsync(sceneName);

            if (!m_SceneHandle.ContainsKey(sceneName))
            {
                m_SceneHandle.Add(sceneName, sceneHandle);
            }
        }

        public void LoadScene(string sceneName, GameFrameworkAction complete)
        {
            var sceneHandle = YooAssets.LoadSceneAsync(sceneName);
            sceneHandle.Completed += (SceneHandle sh) =>
            {
                complete?.Invoke();
            };
            if (!m_SceneHandle.ContainsKey(sceneName))
            {
                m_SceneHandle.Add(sceneName, sceneHandle);
            }
        }

        public AssetHandle LoadAssetAsync<T>(string packageName, string assetName, GameFrameworkAction<T> succ, GameFrameworkAction fail) where T : UnityEngine.Object
        {
            var package = GetPackage(packageName);
            if (package == null)
            {
                return null;
            }
            AssetHandleInfo assetHandleInfo = GetAssetHandleInfo(assetName, typeof(T));
            if (assetHandleInfo != null)
            {
                if (assetHandleInfo.IsDone())
                {
                    if (assetHandleInfo.IsFinish())
                    {
                        succ?.Invoke(assetHandleInfo.GetAssetObject() as T);
                    }
                    else
                    {
                        fail?.Invoke();
                    }
                }
                else
                {
                    AddLoadTask(assetHandleInfo, succ, fail);
                }
                assetHandleInfo.SetLastUsedTime();
                return assetHandleInfo.GetAssetHandle();
            }
            else
            {
                AssetHandle assetHandle = package.LoadAssetAsync<T>(assetName);
                if (!m_AssetHandleMap.TryGetValue(assetName, out var assetHandleInfoList))
                {
                    assetHandleInfoList = new List<AssetHandleInfo>();
                    m_AssetHandleMap.Add(assetName, assetHandleInfoList);
                }
                assetHandleInfo = AssetHandleInfo.Create(m_DefaultPackageName, assetName, typeof(T), assetHandle);
                assetHandleInfoList.Add(assetHandleInfo);
                AddLoadTask(assetHandleInfo, succ, fail);

                return assetHandle;
            }
        }

        public AssetHandle LoadAssetAsync<T>(string assetName, GameFrameworkAction<T> succ) where T : UnityEngine.Object
        {
            return LoadAssetAsync(m_DefaultPackageName, assetName, succ, null);
        }

        public AssetHandle LoadAssetAsync<T>(string assetName, GameFrameworkAction<T> succ, GameFrameworkAction fail) where T : UnityEngine.Object
        {
            return LoadAssetAsync(m_DefaultPackageName, assetName, succ, fail);
        }


        public AssetHandleInfo GetAssetHandleInfo(string assetName, Type type)
        {
            if (m_AssetHandleMap.TryGetValue(assetName, out List<AssetHandleInfo> assetHandleInfoList))
            {
                foreach (var item in assetHandleInfoList)
                {
                    if (item.Equals(assetName, type))
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        public ResourcePackage GetPackage(string packageName)
        {
            m_Packages.TryGetValue(packageName, out ResourcePackage package);
            return package;
        }

        void AddLoadTask<T>(AssetHandleInfo info, GameFrameworkAction<T> succ, GameFrameworkAction fail) where T : UnityEngine.Object
        {
            var assetHandle = info.GetAssetHandle();
            assetHandle.Completed += (AssetHandle ah) =>
            {
                if (ah.AssetObject != null)
                {
                    succ?.Invoke(ah.AssetObject as T);
                }
                else
                {
                    fail?.Invoke();
                }
            };
        }

        /// <summary>
        /// 关闭并清理游戏框架模块。
        /// </summary>
        internal void Shutdown()
        {

        }

    }
}