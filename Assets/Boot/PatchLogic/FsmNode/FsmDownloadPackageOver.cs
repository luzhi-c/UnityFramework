

using UnityGameFramework.Runtime;

namespace GamePlay.Boot
{
    public class FsmDownloadPackageOver : IStateNode
    {
        private IStateMachine m_Machine;

        void IStateNode.OnCreate(IStateMachine machine)
        {
            m_Machine = machine;
        }
        void IStateNode.OnEnter()
        {
            GameEntry.Event.Fire(this, PatchStcepsChangeArgs.Create("资源文件下载完毕！"));
            m_Machine.ChangeState<FsmClearCacheBundle>();
        }
        void IStateNode.OnUpdate()
        {
        }
        void IStateNode.OnExit()
        {
        }
    }
}
