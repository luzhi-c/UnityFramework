using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using YooAsset;

namespace GamePlay.Boot
{
    public class FsmClearCacheBundle : IStateNode
    {
        private IStateMachine m_Machine;

        void IStateNode.OnCreate(IStateMachine machine)
        {
            m_Machine = machine;
        }
        void IStateNode.OnEnter()
        {
            // 触发事件
            GameEntry.Event.Fire(this, PatchStcepsChangeArgs.Create("清理未使用的缓存文件！"));

            var packageName = (string)m_Machine.GetBlackboardValue("PackageName");
            var package = YooAssets.GetPackage(packageName);
            var operation = package.ClearCacheFilesAsync(EFileClearMode.ClearUnusedBundleFiles);
            operation.Completed += Operation_Completed;
        }
        void IStateNode.OnUpdate()
        {
        }
        void IStateNode.OnExit()
        {
        }

        private void Operation_Completed(YooAsset.AsyncOperationBase obj)
        {
            m_Machine.ChangeState<FsmStartGame>();
        }
    }
}
