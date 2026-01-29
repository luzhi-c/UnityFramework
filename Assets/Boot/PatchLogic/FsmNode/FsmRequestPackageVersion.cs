using System.Collections;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
using YooAsset;
namespace GamePlay.Boot
{
    public class FsmRequestPackageVersion : IStateNode
    {
        private IStateMachine m_Machine;

        public void OnCreate(IStateMachine machine)
        {
            m_Machine = machine;
        }
        void IStateNode.OnEnter()
        {
            GameEntry.Event.Fire(this, PatchStcepsChangeArgs.Create("请求资源版本 !"));
            GameEntry.Instance.StartCoroutine(UpdatePackageVersion());
        }
        void IStateNode.OnUpdate()
        {
        }
        void IStateNode.OnExit()
        {
        }

        private IEnumerator UpdatePackageVersion()
        {
            var packageName = (string)m_Machine.GetBlackboardValue("PackageName");
            var package = YooAssets.GetPackage(packageName);
            var operation = package.RequestPackageVersionAsync();
            yield return operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                Log.Warning(operation.Error);
                GameEntry.Event.Fire(this, PackageVersionRequestFailedArgs.Create());
            }
            else
            {
                Log.Debug($"Request package version : {operation.PackageVersion}");
                m_Machine.SetBlackboardValue("PackageVersion", operation.PackageVersion);
                m_Machine.ChangeState<FsmUpdatePackageManifest>();
            }
        }
    }
}
