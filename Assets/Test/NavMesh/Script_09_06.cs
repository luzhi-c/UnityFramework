using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Script_09_06 : MonoBehaviour
{

    public NavMeshAgent navMeshAgent;
    public Transform target;
    private NavMeshPath m_Path;
    // Start is called before the first frame update
    void Start()
    {
        m_Path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, m_Path);
        Debug.Log(m_Path.corners.Length);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < m_Path.corners.Length - 1; i++)
        {
            Debug.DrawLine(m_Path.corners[i], m_Path.corners[i + 1], Color.red);
        }
    }
}
