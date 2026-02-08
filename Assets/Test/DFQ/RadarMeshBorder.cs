using UnityEngine;
using System.Collections.Generic;

public class RadarMeshBorder : MonoBehaviour
{
    [Header("属性值 (0-1)")]
    public float[] values = { 0.8f, 0.6f, 0.9f, 0.5f, 0.7f, 0.8f };
    
    [Header("边框设置")]
    public Color borderColor = new Color(0.2f, 0.6f, 1f, 1f);
    public float size = 2f;
    public float borderThickness = 0.05f;
    
    private void Start()
    {
        // DrawBorderWithMesh();
    }
    
    void DrawBorderWithMesh()
    {
        GameObject borderObj = new GameObject("RadarBorderMesh");
        borderObj.transform.SetParent(transform, false);
        
        MeshFilter meshFilter = borderObj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = borderObj.AddComponent<MeshRenderer>();
        
        Mesh mesh = CreateBorderMesh();
        meshFilter.mesh = mesh;
        
        // 使用Unlit Color材质
        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = borderColor;
        meshRenderer.material = mat;
    }
    
    Mesh CreateBorderMesh()
    {
        Mesh mesh = new Mesh();
        int sides = values.Length;
        
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        
        // 为每条边创建一个四边形（有厚度的线）
        for (int i = 0; i < sides; i++)
        {
            int next = (i + 1) % sides;
            
            // 计算当前边和下一个边的顶点
            Vector2 p1 = GetPoint(i);
            Vector2 p2 = GetPoint(next);
            
            // 计算边的方向
            Vector2 dir = (p2 - p1).normalized;
            Vector2 normal = new Vector2(-dir.y, dir.x); // 垂直方向
            
            // 计算四边形的4个顶点
            Vector2 halfThickness = normal * borderThickness * 0.5f;
            
            Vector3 v1 = new Vector3(p1.x - halfThickness.x, p1.y - halfThickness.y, 0);
            Vector3 v2 = new Vector3(p1.x + halfThickness.x, p1.y + halfThickness.y, 0);
            Vector3 v3 = new Vector3(p2.x - halfThickness.x, p2.y - halfThickness.y, 0);
            Vector3 v4 = new Vector3(p2.x + halfThickness.x, p2.y + halfThickness.y, 0);
            
            int startIdx = vertices.Count;
            
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);
            vertices.Add(v4);
            
            // 两个三角形组成四边形
            triangles.Add(startIdx);
            triangles.Add(startIdx + 1);
            triangles.Add(startIdx + 2);
            
            triangles.Add(startIdx + 1);
            triangles.Add(startIdx + 3);
            triangles.Add(startIdx + 2);
        }
        
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        
        return mesh;
    }
    
    Vector2 GetPoint(int index)
    {
        float angle = 2 * Mathf.PI * index / values.Length - Mathf.PI / 2;
        float x = Mathf.Cos(angle) * size * values[index];
        float y = Mathf.Sin(angle) * size * values[index];
        return new Vector2(x, y);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
            
            for (int i = 0; i < values.Length; i++)
                values[i] = Random.Range(0.3f, 1f);
            
            DrawBorderWithMesh();
        }
    }
}