
using UnityEngine;
using UnityGameFramework.Runtime;

using YooAsset;
namespace GamePlay.Boot
{
    public class FsmCreateDownloader : IStateNode
    {
        private IStateMachine m_Machine;

        void IStateNode.OnCreate(IStateMachine machine)
        {
            m_Machine = machine;
        }
        void IStateNode.OnEnter()
        {
            // 触发事件
            GameEntry.Event.Fire(this, PatchStcepsChangeArgs.Create("创建资源下载器！"));
            CreateDownloader();
        }
        void IStateNode.OnUpdate()
        {
        }
        void IStateNode.OnExit()
        {
        }

        void CreateDownloader()
        {
            var packageName = (string)m_Machine.GetBlackboardValue("PackageName");
            var package = YooAssets.GetPackage(packageName);
            int downloadingMaxNum = 10;
            int failedTryAgain = 3;
            var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
            m_Machine.SetBlackboardValue("Downloader", downloader);

            if (downloader.TotalDownloadCount == 0)
            {
                Log.Debug("Not found any download files !");
                m_Machine.ChangeState<FsmStartGame>();
            }
            else
            {
                // 发现新更新文件后，挂起流程系统
                // 注意：开发者需要在下载前检测磁盘空间不足
                int totalDownloadCount = downloader.TotalDownloadCount;
                long totalDownloadBytes = downloader.TotalDownloadBytes;
                // GEvent.Dispatch(BootEvent.FoundUpdateFiles, FoundUpdateFiles.Create(totalDownloadCount, totalDownloadBytes));
                var sizeMb = totalDownloadBytes / 1048576f;
                sizeMb = Mathf.Clamp(sizeMb, 0.1f, float.MaxValue);
                string totalSizeMb = sizeMb.ToString("f1");
                Log.Debug("Found update patch files, Total count {0} Total size {1}MB", totalDownloadCount, totalSizeMb);
                GameEntry.Event.Fire(this, UserBeginDownloadWebFilesArgs.Create());
            }
        }
    }
}