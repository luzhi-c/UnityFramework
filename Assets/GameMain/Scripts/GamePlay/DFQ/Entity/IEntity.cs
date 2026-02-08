using UnityEngine;

namespace GamePlay.Game
{
    public abstract class IEntity : MonoBehaviour
    {
        protected int m_EntityID;
        public int EntityID { get => m_EntityID; set => m_EntityID = value; }
        public abstract void OnUpdate(float dt);
        public abstract void OnLateUpdate(float dt);
    }
}