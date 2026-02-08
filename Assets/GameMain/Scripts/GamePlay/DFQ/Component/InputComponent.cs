using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamePlay.Game
{
    public enum EInputState
    {
        none,
        pressed,
        hold,
        released
    }
    public class InputComponent : IComponent
    {
        private Dictionary<string, EInputState> m_InputMap = new Dictionary<string, EInputState>();
        public override void OnUpdate(float dt)
        {
            var keys = m_InputMap.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                if (m_InputMap[key] == EInputState.pressed)
                {
                    m_InputMap[key] = EInputState.hold;
                }
                else if (m_InputMap[key] == EInputState.released)
                {
                    m_InputMap[key] = EInputState.none;
                }
            }
        }
        public override void OnLateUpdate(float dt)
        {

        }

        public bool IsPressed(string key)
        {
            return Get(key) == EInputState.pressed;
        }

        public bool IsHold(string key)
        {
            return Get(key) == EInputState.hold;
        }

        public bool IsReleased(string key)
        {
            return Get(key) == EInputState.released;
        }

        public void OnPressed(string key)
        {
            var value = Get(key) != EInputState.none ? EInputState.hold : EInputState.pressed;
            m_InputMap.Set(key, value);
        }

        public bool OnReleased(string key)
        {
            var value = Get(key);
            if (value != EInputState.none && value != EInputState.released)
            {
                m_InputMap.Set(key, EInputState.released);
                return true;
            }
            return false;
        }

        public EInputState Get(string key)
        {
            if (m_InputMap.ContainsKey(key))
            {
                return m_InputMap[key];
            }
            return EInputState.none;
        }
    }
}