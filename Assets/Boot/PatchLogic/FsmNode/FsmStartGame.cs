
using UnityGameFramework.Runtime;

namespace GamePlay.Boot
{
    public class FsmStartGame : IStateNode
    {
        private PatchOperation m_Owner;

        void IStateNode.OnCreate(IStateMachine machine)
        {
            StateMachine stateMachine = machine as StateMachine;
            m_Owner = stateMachine.Owner as PatchOperation;
        }
        void IStateNode.OnEnter()
        {
            // 触发事件
            GameEntry.Event.Fire(this, PatchStcepsChangeArgs.Create("开始游戏！"));
            m_Owner.SetFinish();
        }
        void IStateNode.OnUpdate()
        {
        }
        void IStateNode.OnExit()
        {
        }
    }
}

