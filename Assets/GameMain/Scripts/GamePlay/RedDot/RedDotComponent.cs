using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.RedDOt;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 游戏红点管理
    /// </summary>
    public class RedDotComponent : GameFrameworkComponent
    {
        IRedDotManager m_RedDotManager;

        protected override void Awake()
        {
            base.Awake();
            if (m_RedDotManager == null)
            {
                m_RedDotManager = new RedDotManager();
            }
        }
        public IRedDot Register(int id, int root, GameFrameworkFunc<bool> handler = null)
        {
            if (m_RedDotManager != null)
            {
                return m_RedDotManager.Register(id, root, handler);
            }
            return null;
        }

        public void SetRedDotCalcHandler(int id, GameFrameworkFunc<bool> handler)
        {
            if (m_RedDotManager != null)
            {
                m_RedDotManager.SetRedDotCalcHandler(id, handler);
            }
        }

        public IRedDot GetRedDot(int id)
        {
            if (m_RedDotManager != null)
            {
                return m_RedDotManager.GetRedDot(id);
            }
            return null;
        }

        public bool GetRedDotIsActive(int id)
        {
            if (m_RedDotManager != null)
            {
                return m_RedDotManager.GetRedDotIsActive(id);
            }
            return false;
        }
    }
}