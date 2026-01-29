using GameFramework.Event;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public class Actor : MonoBehaviour, IBaseUI
    {
        private EventGroup m_EventGroup;
        public EventGroup GetEventGroup()
        {
            if (m_EventGroup == null)
            {
                m_EventGroup = new EventGroup();
            }
            return m_EventGroup;
        }

        public void Dispath(GameEventArgs e)
        {
            m_EventGroup.Dispath(e);
        }

        public virtual void Dispose()
        {
            if (m_EventGroup != null)
            {
                m_EventGroup.OffAll();
                m_EventGroup = null;
            }
        }
    }
}