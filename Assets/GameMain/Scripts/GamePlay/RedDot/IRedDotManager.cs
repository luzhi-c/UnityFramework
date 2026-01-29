using System;

namespace GameFramework.RedDOt
{
    /// <summary>
    /// 游戏红点管理
    /// </summary>
    public interface IRedDotManager
    {
        event GameFrameworkAction<RedDotArgs> RedDotChangedEvent;
        IRedDot Register(int id, int root, GameFrameworkFunc<bool> handler = null);
        void SetRedDotCalcHandler(int id, GameFrameworkFunc<bool> handler);
        IRedDot GetRedDot(int id);
        bool GetRedDotIsActive(int id);
        void FireEvent(RedDotArgs e);

    }
}