using System;
using System.Collections.Generic;
namespace GameFramework.RedDOt
{
    public class RedDot : IRedDot
    {
        private int m_ID;
        private int m_RootID;
        private bool m_IsActive;
        private List<int> m_Children;
        private IRedDotManager m_RedDotManager;
        private GameFrameworkFunc<bool> m_RedDotHandler;

        public RedDot(IRedDotManager redDotManager, int id, int root)
        {
            m_Children = new List<int>();
            m_RedDotManager = redDotManager;
            m_ID = id;
            m_RootID = root;
            if (root > 0)
            {
                // 可能父节点不存在
                var rootRedDot = m_RedDotManager.GetRedDot(root);
                if (rootRedDot == null)
                {
                    rootRedDot = redDotManager.Register(root, -1, null);
                }
                rootRedDot?.AddChild(id);
            }
        }

        public override void SetRedDotCalcHandler(GameFrameworkFunc<bool> handler)
        {
            m_RedDotHandler = handler;
        }

        public override void AddChild(int id)
        {
            if (!m_Children.Contains(id))
            {
                m_Children.Add(id);

            }
        }

        public override void CalcRedDot()
        {
            bool changed = false;
            bool isActive = false;
            if (m_RedDotHandler != null)
            {
                isActive = m_RedDotHandler.Invoke();
                if (m_IsActive != isActive)
                {
                    changed = true;
                }
            }
            else
            {
                if (m_Children.Count > 0)
                {

                    // 之前是亮着的 那一定要所有子节点都黑了才算黑
                    // 之前是黑着的 那一定要所有子节点只要亮了一个就算亮
                    for (int i = 0; i < m_Children.Count; i++)
                    {
                        var child = m_RedDotManager.GetRedDot(m_Children[i]);
                        if (child != null)
                        {
                            isActive = isActive || child.IsActive();
                            if (isActive)
                            {
                                break;
                            }
                        }
                    }
                    if (isActive != m_IsActive)
                    {
                        changed = true;
                    }
                }
            }

            if (changed)
            {
                m_IsActive = isActive;
                ChangeRedDotByThis();
                m_RedDotManager.FireEvent(RedDotArgs.Create(m_ID));
            }
        }

        public void ChangeRedDotByThis()
        {
            if (m_RootID > 0)
            {
                var rootRedDot = m_RedDotManager.GetRedDot(m_RootID);
                rootRedDot?.CalcRedDot();
            }
        }

        public override bool IsActive()
        {
            return m_IsActive;
        }
    }
}