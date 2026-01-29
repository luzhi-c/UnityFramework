using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using YooAsset;
namespace GamePlay.Boot
{
    public class PatchOperation : GameAsyncOperation
    {
        private enum ESteps
        {
            None,
            Update,
            Done,
        }

        private EventGroup m_EventGroup;
        private IStateMachine m_Machine;
        private readonly string m_PackageName;
        private ESteps m_Steps = ESteps.None;

        public PatchOperation(string packageName, EPlayMode playMode, bool checkeVersion = true)
        {
            m_EventGroup = new EventGroup();
            m_PackageName = packageName;
            // 注册监听事件
            m_EventGroup.On(UserTryInitializeArgs.EventId, (object sender, GameEventArgs e) => { m_Machine.ChangeState<FsmInitializePackage>(); });
            m_EventGroup.On(UserBeginDownloadWebFilesArgs.EventId, (object sender, GameEventArgs e) => { m_Machine.ChangeState<FsmDownloadPackageFiles>(); });
            m_EventGroup.On(UserTryRequestPackageVersionArgs.EventId, (object sender, GameEventArgs e) => { m_Machine.ChangeState<FsmRequestPackageVersion>(); });
            m_EventGroup.On(UserTryUpdatePackageManifestArgs.EventId, (object sender, GameEventArgs e) => { m_Machine.ChangeState<FsmUpdatePackageManifest>(); });
            m_EventGroup.On(UserTryDownloadWebFilesArgs.EventId, (object sender, GameEventArgs e) => { m_Machine.ChangeState<FsmCreateDownloader>(); });

            // 创建状态机
            m_Machine = new StateMachine(this);
            m_Machine.AddNode<FsmInitializePackage>();
            m_Machine.AddNode<FsmRequestPackageVersion>();
            m_Machine.AddNode<FsmUpdatePackageManifest>();
            m_Machine.AddNode<FsmCreateDownloader>();
            m_Machine.AddNode<FsmDownloadPackageFiles>();
            m_Machine.AddNode<FsmDownloadPackageOver>();
            m_Machine.AddNode<FsmClearCacheBundle>();
            m_Machine.AddNode<FsmStartGame>();

            m_Machine.SetBlackboardValue("PackageName", packageName);
            m_Machine.SetBlackboardValue("PlayMode", playMode);
            m_Machine.SetBlackboardValue("CheckeVersion", checkeVersion);
        }
        protected override void OnStart()
        {
            m_Steps = ESteps.Update;
            m_Machine.Run<FsmInitializePackage>();
        }
        protected override void OnUpdate()
        {
            if (m_Steps == ESteps.None || m_Steps == ESteps.Done)
            {
                return;
            }

            if (m_Steps == ESteps.Update)
            {
                m_Machine.OnUpdate();
            }
        }
        protected override void OnAbort()
        {
        }

        public void SetFinish()
        {
            m_Steps = ESteps.Done;
            m_EventGroup.Clear();
            m_EventGroup = null;
            Status = EOperationStatus.Succeed;
            Log.Debug($"Package {m_PackageName} patch done !");
        }

    }
}