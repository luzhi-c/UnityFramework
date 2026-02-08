using UnityEngine;
using System.Collections.Generic;
using GamePlay.Game;

public class RealRadarChart : MonoBehaviour, IGL
{
    private GameObject radarObj;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh mesh;
    private Material mat;

    List<Vector3> vertices = new();
    List<int> triangles = new();
    List<Color> vertexColors = new(); // 新增：存储每个顶点的颜色
    private float currentLineWidth = 0.1f;

    // 当前线条的颜色缓存
    private Color currentColor = Color.white;

    private void Start()
    {
        radarObj = new GameObject("RadarMesh");
        radarObj.transform.SetParent(transform, false);

        meshFilter = radarObj.AddComponent<MeshFilter>();
        meshRenderer = radarObj.AddComponent<MeshRenderer>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;
        mat = new Material(Shader.Find("Sprites/Default"));
        meshRenderer.material = mat;
    }

    /// <summary>
    /// 设置当前绘制颜色
    /// </summary>
    public void SetColor(Color color)
    {
        currentColor = color;
    }

    public void SetLineWidth(float width)
    {
        currentLineWidth = width;
    }

    /// <summary>
    /// 绘制线段（使用当前颜色）
    /// </summary>
    public void DrawLine(Vector2 startPoint, Vector2 endPoint)
    {
        DrawLine(startPoint, endPoint, currentColor);
    }

    /// <summary>
    /// 绘制线段（指定颜色）
    /// </summary>
    public void DrawLine(Vector2 startPoint, Vector2 endPoint, Color color)
    {
        // 计算线条方向
        Vector2 direction = (endPoint - startPoint).normalized;

        // 计算垂直于线条方向的向量（用于创建宽度）
        Vector2 perpendicular = new Vector2(-direction.y, direction.x) * (currentLineWidth * 0.5f);
        int startIndex = vertices.Count;

        // 添加四个顶点
        vertices.Add(new Vector3(startPoint.x - perpendicular.x, startPoint.y - perpendicular.y, 0));
        vertices.Add(new Vector3(startPoint.x + perpendicular.x, startPoint.y + perpendicular.y, 0));
        vertices.Add(new Vector3(endPoint.x + perpendicular.x, endPoint.y + perpendicular.y, 0));
        vertices.Add(new Vector3(endPoint.x - perpendicular.x, endPoint.y - perpendicular.y, 0));

        // 为四个顶点添加相同的颜色
        for (int i = 0; i < 4; i++)
        {
            vertexColors.Add(color);
        }

        // 添加三角形索引
        triangles.AddRange(new int[] {
            startIndex, startIndex + 2, startIndex + 1,
            startIndex, startIndex + 3, startIndex + 2
        });
    }

    /// <summary>
    /// 绘制渐变颜色的线条（从startColor渐变到endColor）
    /// </summary>
    public void DrawLineGradient(Vector2 startPoint, Vector2 endPoint, Color startColor, Color endColor)
    {
        // 计算线条方向
        Vector2 direction = (endPoint - startPoint).normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x) * (currentLineWidth * 0.5f);
        int startIndex = vertices.Count;

        // 添加四个顶点
        vertices.Add(new Vector3(startPoint.x - perpendicular.x, startPoint.y - perpendicular.y, 0));
        vertices.Add(new Vector3(startPoint.x + perpendicular.x, startPoint.y + perpendicular.y, 0));
        vertices.Add(new Vector3(endPoint.x + perpendicular.x, endPoint.y + perpendicular.y, 0));
        vertices.Add(new Vector3(endPoint.x - perpendicular.x, endPoint.y - perpendicular.y, 0));

        // 为四个顶点设置渐变颜色
        // 左下角和右下角用起始颜色
        vertexColors.Add(startColor);
        vertexColors.Add(startColor);
        // 右上角和左上角用结束颜色
        vertexColors.Add(endColor);
        vertexColors.Add(endColor);

        // 添加三角形索引
        triangles.AddRange(new int[] {
            startIndex, startIndex + 2, startIndex + 1,
            startIndex, startIndex + 3, startIndex + 2
        });
    }

    /// <summary>
    /// 绘制带圆角的线条连接
    /// </summary>
    public void DrawLineWithRoundJoin(Vector2 pointA, Vector2 pointB, Vector2 pointC, Color color)
    {
        // 先画第一条线
        DrawLine(pointA, pointB, color);

        // 再画第二条线
        DrawLine(pointB, pointC, color);

        // 这里可以添加圆角连接的几何体（需要额外的顶点）
        // 简化实现：可以在连接处绘制一个小圆形
    }

    /// <summary>
    /// 绘制虚线
    /// </summary>
    public void DrawDashedLine(Vector2 startPoint, Vector2 endPoint, Color color, float dashLength = 0.5f, float gapLength = 0.3f)
    {
        Vector2 direction = (endPoint - startPoint).normalized;
        float totalLength = Vector2.Distance(startPoint, endPoint);
        Vector2 currentPos = startPoint;

        while (true)
        {
            float remainingLength = Vector2.Distance(currentPos, endPoint);
            if (remainingLength <= dashLength)
            {
                // 最后一段
                DrawLine(currentPos, endPoint, color);
                break;
            }

            Vector2 dashEnd = currentPos + direction * dashLength;
            DrawLine(currentPos, dashEnd, color);

            currentPos = dashEnd + direction * gapLength;

            if (Vector2.Distance(currentPos, endPoint) <= 0)
                break;
        }
    }

    public void End()
    {
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.colors = vertexColors.ToArray(); // 设置顶点颜色
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    /// <summary>
    /// 清除所有绘制内容
    /// </summary>
    public void Clear()
    {
        vertices.Clear();
        triangles.Clear();
        vertexColors.Clear();
        mesh.Clear();
    }

    /// <summary>
    /// 开始新的绘制批次（清除之前的内容）
    /// </summary>
    public void Begin()
    {
        Clear();
    }
}