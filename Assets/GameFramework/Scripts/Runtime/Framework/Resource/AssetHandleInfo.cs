using System;
using GameFramework;
using YooAsset;

namespace UnityGameFramework.Runtime
{
    public class AssetHandleInfo : IReference
    {
        private string m_PackageName;
        private string m_AssetName;
        private Type m_AssetType;
        private AssetHandle m_AssetHandle;
        private int m_LastUsedTime = 0;
        private int m_RefrenceCount = 0;

        public static AssetHandleInfo Create(string packageName, string assetName, Type assetType, AssetHandle assetHandle)
        {
            AssetHandleInfo info = ReferencePool.Acquire<AssetHandleInfo>();
            info.m_PackageName = packageName;
            info.m_AssetName = assetName;
            info.m_AssetType = assetType;
            info.m_AssetHandle = assetHandle;
            info.m_LastUsedTime = DateTime.UtcNow.Second;
            info.m_RefrenceCount = 0;
            return info;
        }
        public void Clear()
        {
            m_PackageName = null;
            m_AssetName = null;
            m_AssetType = null;
            m_AssetHandle = null;
            m_LastUsedTime = 0;
            m_RefrenceCount = 0;
        }

        public AssetHandle GetAssetHandle()
        {
            return m_AssetHandle;
        }

        public void SetLastUsedTime()
        {
            m_LastUsedTime = DateTime.UtcNow.Second;
        }

        public bool IsDone()
        {
            return m_AssetHandle != null && m_AssetHandle.IsDone;
        }

        public bool IsFinish()
        {
            return IsDone() && m_AssetHandle.AssetObject != null;
        }

        public UnityEngine.Object GetAssetObject()
        {
            if (m_AssetHandle == null)
            {
                return null;
            }
            return m_AssetHandle.AssetObject;
        }

        public void AddRefrence()
        {
            m_RefrenceCount++;
        }

        public void SubRefrence()
        {
            m_RefrenceCount--;
        }

        public bool Equals(string assetName, Type type)
        {
            return m_AssetName == assetName && m_AssetType == type;
        }

        public bool CanRelease()
        {
            if (m_RefrenceCount <= 0)
            {
                return true;
            }
            return false;
        }

        public bool TryReleaseByTime(int time)
        {
            if (CanRelease())
            {
                if (DateTime.UtcNow.Second - m_LastUsedTime >= time)
                {
                    Release();
                    return true;
                }
            }
            return false;
        }

        public void Release()
        {
            m_AssetHandle.Release();
        }

    }
}