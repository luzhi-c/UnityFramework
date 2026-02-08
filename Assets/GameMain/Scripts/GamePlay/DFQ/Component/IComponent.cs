using UnityEngine;

namespace GamePlay.Game
{
    public abstract class IComponent : MonoBehaviour
    {
        public abstract void OnUpdate(float dt);
        public abstract void OnLateUpdate(float dt);
    }
}