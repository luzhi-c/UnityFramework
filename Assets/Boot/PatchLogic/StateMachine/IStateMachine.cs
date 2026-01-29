using System;

namespace GamePlay.Boot
{
    public interface IStateMachine
    {

        /// <summary>
        /// 更新状态机
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// 启动状态机
        /// </summary>
        void Run<TNode>() where TNode : IStateNode;
        void Run(Type entryNode);
        void Run(string entryNode);

        /// <summary>
        /// 加入一个节点
        /// </summary>
        void AddNode<TNode>() where TNode : IStateNode;
        void AddNode(IStateNode stateNode);

        /// <summary>
        /// 转换状态节点
        /// </summary>
        void ChangeState<TNode>() where TNode : IStateNode;

        void ChangeState(Type nodeType);

        void ChangeState(string nodeName);
        /// <summary>
        /// 设置黑板数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SetBlackboardValue(string key, System.Object value);

        /// <summary>
        /// 获取黑板数据
        /// </summary>
        System.Object GetBlackboardValue(string key);
        IStateNode TryGetNode(string nodeName);
    }
}