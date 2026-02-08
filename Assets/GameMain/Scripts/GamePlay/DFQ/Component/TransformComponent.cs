using System.ComponentModel;
using UnityEngine;

namespace GamePlay.Game
{
    public class TransformComponent : IComponent
    {
        public bool positionTick = false;
        public bool scaleTick = false;
        public bool radianTick = false;
        public Vector3 position = new Vector3();
        public Vector3 scale = new Vector3();
        public int direction = 1;
        public override void OnUpdate(float dt)
        {
            
        }

        public override void OnLateUpdate(float dt)
        {
            var colliderTick = positionTick || scaleTick || radianTick;

            if (positionTick)
            {
                var x = position.x;
                var y = position.y + position.z;
                // y轴相反
                gameObject.transform.localPosition = new Vector3(x, -y);
                positionTick = false;
            }

            if (colliderTick)
            {

            }
        }
    }
}