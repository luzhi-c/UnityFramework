using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SimplePolygonBorder : MonoBehaviour
{
    public float[] values = { 0.8f, 0.6f, 0.9f, 0.5f, 0.7f, 0.8f };
    public float size = 2f;
    public Color color = Color.cyan;
    
    void Start()
    {
        UpdateBorder();
    }
    
    void UpdateBorder()
    {
        Mesh mesh = new Mesh();
        int sides = values.Length;
        
        // 顶点：多边形顶点
        Vector3[] vertices = new Vector3[sides];
        for (int i = 0; i < sides; i++)
        {
            float angle = 2 * Mathf.PI * i / sides - Mathf.PI / 2;
            vertices[i] = new Vector3(
                Mathf.Cos(angle) * size * values[i],
                Mathf.Sin(angle) * size * values[i],
                0
            );
        }
        
        // 边：每两个顶点构成一条线
        // 使用Line primitive需要特殊处理，这里我们用三角形模拟线
        // 创建一个很细的三角形带来模拟边框
        List<Vector3> lineVertices = new List<Vector3>();
        List<int> lineTriangles = new List<int>();
        
        float thickness = 0.02f;
        
        for (int i = 0; i < sides; i++)
        {
            int j = (i + 1) % sides;
            Vector3 a = vertices[i];
            Vector3 b = vertices[j];
            
            Vector3 dir = (b - a).normalized;
            Vector3 normal = new Vector3(-dir.y, dir.x, 0);
            
            Vector3 a1 = a - normal * thickness * 0.5f;
            Vector3 a2 = a + normal * thickness * 0.5f;
            Vector3 b1 = b - normal * thickness * 0.5f;
            Vector3 b2 = b + normal * thickness * 0.5f;
            
            int startIdx = lineVertices.Count;
            
            lineVertices.Add(a1);
            lineVertices.Add(a2);
            lineVertices.Add(b1);
            lineVertices.Add(b2);
            
            // 两个三角形
            lineTriangles.Add(startIdx);
            lineTriangles.Add(startIdx + 1);
            lineTriangles.Add(startIdx + 2);
            
            lineTriangles.Add(startIdx + 1);
            lineTriangles.Add(startIdx + 3);
            lineTriangles.Add(startIdx + 2);
        }
        
        mesh.vertices = lineVertices.ToArray();
        mesh.triangles = lineTriangles.ToArray();
        mesh.RecalculateNormals();
        
        GetComponent<MeshFilter>().mesh = mesh;
        
        // 设置材质
        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = color;
        GetComponent<MeshRenderer>().material = mat;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < values.Length; i++)
                values[i] = Random.Range(0.3f, 1f);
            
            UpdateBorder();
        }
    }
}