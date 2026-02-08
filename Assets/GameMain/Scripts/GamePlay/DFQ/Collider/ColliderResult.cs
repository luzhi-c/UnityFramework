using GameFramework;
using UnityEngine;

namespace GamePlay.Game
{
    public class ColliderResult : IReference
    {
        private bool m_IsHit;
        private float m_X;
        private float m_Y;
        private float m_Z;

        public static ColliderResult Create(bool isHit = false, float x = 0, float y = 0, float z = 0)
        {
            var result = ReferencePool.Acquire<ColliderResult>();
            result.m_IsHit = isHit;
            result.m_X = x;
            result.m_Y = y;
            result.m_Z = z;
            return result;
        }

        public bool IsHit
        {
            get { return m_IsHit; }
            set { m_IsHit = value; }
        }

        public float X
        {
            set { m_X = value; }
            get { return m_X; }
        }

        public float Y
        {
            set { m_Y = value; }
            get { return m_Y; }
        }

        public float Z
        {
            set { m_Z = value; }
            get { return m_Z; }
        }

        public void Clear()
        {
            m_IsHit = false;
            m_X = 0;
            m_Y = 0;
            m_Z = 0;
        }
    }
}