using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using YooAsset;

namespace GamePlay.Boot
{
    public class FsmUpdatePackageManifest : IStateNode
    {
        private IStateMachine m_Machine;

        public void OnCreate(IStateMachine machine)
        {
            m_Machine = machine;
        }
        void IStateNode.OnEnter()
        {
            GameEntry.Event.Fire(this, PatchStcepsChangeArgs.Create("更新资源清单 !"));
            GameEntry.Instance.StartCoroutine(UpdateManifest());
        }
        void IStateNode.OnUpdate()
        {
        }
        void IStateNode.OnExit()
        {
        }

        private IEnumerator UpdateManifest()
        {
            var packageName = (string)m_Machine.GetBlackboardValue("PackageName");
            var packageVersion = (string)m_Machine.GetBlackboardValue("PackageVersion");
            var package = YooAssets.GetPackage(packageName);
            var operation = package.UpdatePackageManifestAsync(packageVersion);
            yield return operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                Log.Warning(operation.Error);
                GameEntry.Event.Fire(this, PackageManifestUpdateFailedArgs.Create());
                yield break;
            }
            else
            {
                m_Machine.ChangeState<FsmCreateDownloader>();
            }
        }
    }
}
