using System.Collections;
using System.Collections.Generic;
using RVO;
using UnityEngine;
using UnityGameFramework.Runtime;
using Vector2 = RVO.Vector2;

namespace Test
{
    public class RVO2Manager : SingletonBehaviour<RVO2Manager>
    {
        public GameObject agentPrefab;
        public Plane hPlane;
        [HideInInspector] public Vector2 mousePosition = new Vector2();


        private Dictionary<int, GameAgent> m_agentMap = new Dictionary<int, GameAgent>();

        // Use this for initialization
        void Start()
        {
            Simulator.Instance.setTimeStep(0.25f);
            Simulator.Instance.setAgentDefaults(1f, 20, 5.0f, 5.0f, 2.0f, 2.0f, new RVO.Vector2(0.0f, 0.0f));

            // add in awake
            Simulator.Instance.processObstacles();
        }

        private void UpdateMousePosition()
        {

            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out RaycastHit hitInfo, 100f, LayerMask.NameToLayer("Plane")))
            {
                Vector3 position = hitInfo.point;
                mousePosition = new Vector2(position.x, position.z);
            }

        }

        void DeleteAgent()
        {
            int agentNo = Simulator.Instance.queryNearAgent(mousePosition, 1.5f);
            if (agentNo == -1 || !m_agentMap.ContainsKey(agentNo))
                return;

            Simulator.Instance.delAgent(agentNo);
            Destroy(m_agentMap[agentNo].gameObject);
            m_agentMap.Remove(agentNo);
        }

        void CreatAgent()
        {
            int sid = Simulator.Instance.addAgent(mousePosition);
            if (sid >= 0)
            {
                GameObject go = Instantiate(agentPrefab, new Vector3(mousePosition.x(), 0, mousePosition.y()), Quaternion.identity);
                GameAgent ga = go.GetComponent<GameAgent>();
                go.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                ga.sid = sid;
                m_agentMap.Add(sid, ga);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateMousePosition();
            if (Input.GetMouseButtonUp(0))
            {
                if (Input.GetKey(KeyCode.Delete))
                {
                    DeleteAgent();
                }
                else
                {
                    CreatAgent();
                }
            }

            Simulator.Instance.doStep();
        }
    }
}

