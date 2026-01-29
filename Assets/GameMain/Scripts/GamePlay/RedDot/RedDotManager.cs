using System;
using System.Collections.Generic;

namespace GameFramework.RedDOt
{
    /// <summary>
    /// 游戏红点管理
    /// </summary>
    public class RedDotManager : IRedDotManager
    {
        private Dictionary<int, IRedDot> m_RedDotMap;

        private event GameFrameworkAction<RedDotArgs> m_RedDotChangedEventHandler;

        public event GameFrameworkAction<RedDotArgs> RedDotChangedEvent
        {
            add
            {
                m_RedDotChangedEventHandler += value;
            }
            remove
            {
                m_RedDotChangedEventHandler -= value;
            }
        }
        public RedDotManager()
        {
            m_RedDotMap = new();
        }
        public IRedDot Register(int id, int root, GameFrameworkFunc<bool> handler = null)
        {
            if (m_RedDotMap.TryGetValue(id, out var redDot))
            {
                return redDot;
            }
            redDot = new RedDot(this, id, root);
            redDot.SetRedDotCalcHandler(handler);
            m_RedDotMap.Add(id, redDot);
            // 初始计算红点
            redDot.CalcRedDot();
            return redDot;
        }

        public void SetRedDotCalcHandler(int id, GameFrameworkFunc<bool> handler)
        {
            IRedDot redDot = GetRedDot(id);
            if (redDot != null)
            {
                redDot.SetRedDotCalcHandler(handler);
            }

        }

        public IRedDot GetRedDot(int id)
        {
            if (m_RedDotMap.ContainsKey(id))
            {
                return m_RedDotMap[id];
            }
            return null;
        }

        public bool GetRedDotIsActive(int id)
        {
            IRedDot redDot = GetRedDot(id);
            if (redDot != null)
            {
                return redDot.IsActive();
            }
            return false;
        }

        public void FireEvent(RedDotArgs e)
        {
            m_RedDotChangedEventHandler(e);
        }
    }
}