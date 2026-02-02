using System.Collections;
using System.Collections.Generic;
using RVO;
using UnityEngine;

namespace Test
{
    public class GameAgent : MonoBehaviour
    {
        public int sid = -1;
        private System.Random m_random = new System.Random();
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (sid >= 0)
            {
                RVO.Vector2 pos = Simulator.Instance.getAgentPosition(sid);
                RVO.Vector2 vel = Simulator.Instance.getAgentPrefVelocity(sid);
                transform.position = new Vector3(pos.x(), transform.position.y, pos.y());
                // if (Mathf.Abs(vel.x()) > 0.01f && Mathf.Abs(vel.y()) > 0.01f)
                //     transform.forward = new Vector3(vel.x(), 0, vel.y()).normalized;
            }

            if (!Input.GetMouseButton(1))
            {
                Simulator.Instance.setAgentPrefVelocity(sid, new RVO.Vector2(0, 0));
                return;
            }

            RVO.Vector2 goalVector = RVO2Manager.Instance.mousePosition - Simulator.Instance.getAgentPosition(sid);
            if (RVOMath.absSq(goalVector) > 1.0f)
            {
                goalVector = RVOMath.normalize(goalVector);
            }

            Simulator.Instance.setAgentPrefVelocity(sid, goalVector * (Time.deltaTime * 10));

            /* Perturb a little to avoid deadlocks due to perfect symmetry. */
            // float angle = (float)m_random.NextDouble() * 2.0f * (float)Mathf.PI;
            // float dist = (float)m_random.NextDouble() * 0.0001f;

            // Simulator.Instance.setAgentPrefVelocity(sid, Simulator.Instance.getAgentPrefVelocity(sid) +
            //                                              dist *
            //                                              new RVO.Vector2((float)Mathf.Cos(angle), (float)Mathf.Sin(angle)));

        }
    }
}

