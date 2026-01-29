using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    public class EventGroup
    {
        private EventComponent m_Event;
        private Dictionary<int, EventHandler<GameEventArgs>> m_EventMap = new();
        public EventGroup()
        {
            m_Event = GameEntry.GetComponent<EventComponent>();
        }
        public void On(int eventId, EventHandler<GameEventArgs> handler)
        {
            if (m_EventMap.ContainsKey(eventId))
            {
                Log.Warning(Utility.Text.Format("监听同一个事件{0}", eventId));
                return;
            }
            m_Event.Subscribe(eventId, handler);
            m_EventMap.Add(eventId, handler);
        }

        public void Off(int eventId, EventHandler<GameEventArgs> handler)
        {
            if (m_EventMap.ContainsKey(eventId))
            {
                m_Event.Unsubscribe(eventId, handler);
                m_EventMap.Remove(eventId);
            }

        }

        public void Dispath(GameEventArgs e)
        {
            m_Event.Fire(this, e);
        }

        public void OffAll()
        {
            foreach (var item in m_EventMap)
            {
                m_Event.Unsubscribe(item.Key, item.Value);
            }
            m_EventMap.Clear();
        }

        public void Clear()
        {
            OffAll();
            m_Event = null;
            m_EventMap = null;
        }
    }
}