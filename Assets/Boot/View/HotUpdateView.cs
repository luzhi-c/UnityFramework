using UnityEngine;
using UnityGameFramework.Runtime;
using YooAsset;

namespace GamePlay.Boot
{
    public class HotUpdateView : UIFormLogic
    {
        enum EHotUpdateStep
        {
            Init,
            UpdatePackageVersion,
            UpdatePackageManifest,
            CreateDownloader,
            ClearCacheBundle,
            Finish,
        }
        private string m_PackageName;
        private ResourcePackage m_Package;
        private EHotUpdateStep m_CurrentStep = EHotUpdateStep.Init;
        private InitializationOperation m_InitializationOperation;
        private RequestPackageVersionOperation m_RequestPackageVersionOperation;
        private string m_PackageVersion;
        private UpdatePackageManifestOperation m_UpdatePackageManifestOperation;


        private ResourceDownloaderOperation m_Downloader;
        private ClearCacheFilesOperation m_ClearCacheFilesOperation;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_PackageName = GameStatic.DefaultPackage;
            StartYooassetHotUpdate();
        }

        void StartYooassetHotUpdate()
        {
            InitializationOperation();
        }

        void InitializationOperation()
        {
            m_CurrentStep = EHotUpdateStep.Init;
            // 创建资源包裹类
            m_Package = YooAssets.TryGetPackage(m_PackageName);
            if (m_Package == null)
            {
                m_Package = YooAssets.CreatePackage(m_PackageName);
            }
            string defaultHostServer = GetHostServerURL(GameStatic.HotUpdateUrl, GameStatic.AppVersion);
            string fallbackHostServer = GetHostServerURL(GameStatic.HotUpdateUrl, GameStatic.AppVersion);
            IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
            var createParameters = new HostPlayModeParameters();
            createParameters.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
            createParameters.CacheFileSystemParameters = FileSystemParameters.CreateDefaultCacheFileSystemParameters(remoteServices);
            m_InitializationOperation = m_Package.InitializeAsync(createParameters);
        }

        void UpdatePackageVersion()
        {
            m_CurrentStep = EHotUpdateStep.UpdatePackageVersion;
            m_RequestPackageVersionOperation = m_Package.RequestPackageVersionAsync();
        }

        void UpdatePackageManifest()
        {
            m_CurrentStep = EHotUpdateStep.UpdatePackageManifest;
            m_UpdatePackageManifestOperation = m_Package.UpdatePackageManifestAsync(m_PackageVersion);
        }

        void CreateDownloader()
        {
            m_CurrentStep = EHotUpdateStep.CreateDownloader;
            int downloadingMaxNum = 10;
            int failedTryAgain = 3;
            m_Downloader = m_Package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);

            if (m_Downloader.TotalDownloadCount == 0)
            {
                m_CurrentStep = EHotUpdateStep.Finish;
            }
            else
            {
                // 发现新更新文件后，挂起流程系统
                // 注意：开发者需要在下载前检测磁盘空间不足
                int totalDownloadCount = m_Downloader.TotalDownloadCount;
                long totalDownloadBytes = m_Downloader.TotalDownloadBytes;
                // GEvent.Dispatch(BootEvent.FoundUpdateFiles, FoundUpdateFiles.Create(totalDownloadCount, totalDownloadBytes));
                var sizeMb = totalDownloadBytes / 1048576f;
                sizeMb = Mathf.Clamp(sizeMb, 0.1f, float.MaxValue);
                string totalSizeMb = sizeMb.ToString("f1");
                Log.Debug("Found update patch files, Total count {0} Total size {1}MB", totalDownloadCount, totalSizeMb);
                m_Downloader.DownloadErrorCallback = DownloadErrorCallback;
                m_Downloader.DownloadUpdateCallback = DownloadUpdateCallback;
                m_Downloader.BeginDownload();
            }
        }

        void DownloadErrorCallback(YooAsset.DownloadErrorData data)
        {
            GameEntry.Event.Fire(this, WebFileDownloadFailedArgs.Create(data));
        }

        void DownloadUpdateCallback(YooAsset.DownloadUpdateData data)
        {
            GameEntry.Event.Fire(this, DownloadUpdateArgs.Create(data));
        }

        void ClearCacheBundle()
        {
            m_CurrentStep = EHotUpdateStep.ClearCacheBundle;
            m_ClearCacheFilesOperation = m_Package.ClearCacheFilesAsync(EFileClearMode.ClearUnusedBundleFiles);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (m_CurrentStep == EHotUpdateStep.Init)
            {
                if (!m_InitializationOperation.IsDone)
                {
                    return;
                }
                // 如果初始化失败弹出提示界面
                if (m_InitializationOperation.Status != EOperationStatus.Succeed)
                {
                    Log.Warning($"{m_InitializationOperation.Error}");
                    GameEntry.Event.Fire(this, InitializeFailedArgs.Create());
                }
                else
                {
                    UpdatePackageVersion();
                }
            }
            else if (m_CurrentStep == EHotUpdateStep.UpdatePackageVersion)
            {
                if (!m_RequestPackageVersionOperation.IsDone)
                {
                    return;
                }
                if (m_RequestPackageVersionOperation.Status != EOperationStatus.Succeed)
                {
                    Log.Warning(m_RequestPackageVersionOperation.Error);
                    GameEntry.Event.Fire(this, PackageVersionRequestFailedArgs.Create());
                }
                else
                {
                    Log.Debug($"Request package version : {m_RequestPackageVersionOperation.PackageVersion}");
                    m_PackageVersion = m_RequestPackageVersionOperation.PackageVersion;
                    UpdatePackageManifest();
                }
            }
            else if (m_CurrentStep == EHotUpdateStep.UpdatePackageManifest)
            {
                if (!m_UpdatePackageManifestOperation.IsDone)
                {
                    return;
                }
                if (m_UpdatePackageManifestOperation.Status != EOperationStatus.Succeed)
                {
                    Log.Warning(m_UpdatePackageManifestOperation.Error);
                    GameEntry.Event.Fire(this, PackageManifestUpdateFailedArgs.Create());
                }
                else
                {
                    CreateDownloader();
                }
            }
            else if (m_CurrentStep == EHotUpdateStep.CreateDownloader)
            {
                if (!m_Downloader.IsDone)
                {
                    return;
                }
                if (m_Downloader.Status != EOperationStatus.Succeed)
                {

                }
                else
                {
                    ClearCacheBundle();
                }
            }
            else if (m_CurrentStep == EHotUpdateStep.ClearCacheBundle)
            {
                if (!m_ClearCacheFilesOperation.IsDone)
                {
                    return;
                }

                if (m_ClearCacheFilesOperation.Status != EOperationStatus.Succeed)
                {
                    Log.Warning("清理缓存失败");
                }
                m_CurrentStep = EHotUpdateStep.Finish;
            }

        }

        /// <summary>
        /// 获取资源服务器地址
        /// </summary>
        private string GetHostServerURL(string hostServerIP, string appVersion)
        {
            // string hostServerIP = "http://10.0.2.2"; //安卓模拟器地址
#if UNITY_EDITOR
            if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
                return $"{hostServerIP}/CDN/Android/{appVersion}";
            else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
                return $"{hostServerIP}/CDN/IPhone/{appVersion}";
            else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
                return $"{hostServerIP}/CDN/WebGL/{appVersion}";
            else
                return $"{hostServerIP}/CDN/PC/{appVersion}";
#else
        if (Application.platform == RuntimePlatform.Android)
            return $"{hostServerIP}/CDN/Android/{appVersion}";
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            return $"{hostServerIP}/CDN/IPhone/{appVersion}";
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
            return $"{hostServerIP}/CDN/WebGL/{appVersion}";
        else
            return $"{hostServerIP}/CDN/PC/{appVersion}";
#endif
        }

        /// <summary>
        /// 远端资源地址查询服务类
        /// </summary>
        private class RemoteServices : IRemoteServices
        {
            private readonly string _defaultHostServer;
            private readonly string _fallbackHostServer;

            public RemoteServices(string defaultHostServer, string fallbackHostServer)
            {
                _defaultHostServer = defaultHostServer;
                _fallbackHostServer = fallbackHostServer;
            }
            string IRemoteServices.GetRemoteMainURL(string fileName)
            {
                return $"{_defaultHostServer}/{fileName}";
            }
            string IRemoteServices.GetRemoteFallbackURL(string fileName)
            {
                return $"{_fallbackHostServer}/{fileName}";
            }
        }
    }
}