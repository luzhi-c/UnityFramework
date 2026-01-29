using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public class PopupOptions : IReference
    {
        public bool m_IsForbidCovered;
        public bool m_IsNotCoverPrePopup;

        public static PopupOptions Create(bool isForbidCovered, bool isNotCoverPrePopup)
        {
            var data = ReferencePool.Acquire<PopupOptions>();
            data.m_IsForbidCovered = isForbidCovered;
            data.m_IsNotCoverPrePopup = isNotCoverPrePopup;
            return data;
        }

        public void Clear()
        {
            m_IsForbidCovered = false;
            m_IsNotCoverPrePopup = false;
        }
    }

    public class PopupInfo : IReference
    {
        private string m_ViewName;
        private PopupViewMediator m_Mediator;
        private int m_ZOrder;
        private PopupOptions m_Options;

        public static PopupInfo Create(string viewName, PopupViewMediator mediator, int zorder, PopupOptions options)
        {
            var data = ReferencePool.Acquire<PopupInfo>();
            data.m_ViewName = viewName;
            data.m_Mediator = mediator;
            data.m_ZOrder = zorder;
            data.m_Options = options;
            return data;
        }

        public int ZOrder
        {
            set => m_ZOrder = value;
            get => m_ZOrder;
        }

        public PopupViewMediator Mediator
        {
            set => m_Mediator = value;
            get => m_Mediator;
        }

        public string ViewName
        {
            get => m_ViewName;
        }

        public void Clear()
        {
            m_ViewName = null;
            m_Mediator = null;
            m_ZOrder = 0;
            m_Options = null;
        }
    }
}