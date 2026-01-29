using System.Collections;
using UnityEngine;
using UnityGameFramework.Runtime;
using YooAsset;

namespace GamePlay.Boot
{
    public class FsmDownloadPackageFiles : IStateNode
    {
        private IStateMachine m_Machine;

        void IStateNode.OnCreate(IStateMachine machine)
        {
            m_Machine = machine;
        }
        void IStateNode.OnEnter()
        {
            // 触发事件
            GameEntry.Event.Fire(this, PatchStcepsChangeArgs.Create("开始下载资源文件！"));
            GameEntry.Instance.StartCoroutine(BeginDownload());
        }
        void IStateNode.OnUpdate()
        {
        }
        void IStateNode.OnExit()
        {
        }

        void DownloadErrorCallback(YooAsset.DownloadErrorData data)
        {
            GameEntry.Event.Fire(this, WebFileDownloadFailedArgs.Create(data));
        }

        void DownloadUpdateCallback(YooAsset.DownloadUpdateData data)
        {
            GameEntry.Event.Fire(this, DownloadUpdateArgs.Create(data));
        }

        private IEnumerator BeginDownload()
        {
            var downloader = (ResourceDownloaderOperation)m_Machine.GetBlackboardValue("Downloader");
            downloader.DownloadErrorCallback = DownloadErrorCallback;
            downloader.DownloadUpdateCallback = DownloadUpdateCallback;
            downloader.BeginDownload();
            yield return downloader;

            // 检测下载结果
            if (downloader.Status != EOperationStatus.Succeed)
                yield break;

            m_Machine.ChangeState<FsmDownloadPackageOver>();
        }
    }
}
