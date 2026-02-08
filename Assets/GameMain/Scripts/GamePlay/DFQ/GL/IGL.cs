using UnityEngine;

namespace GamePlay.Game
{
    public interface IGL
    {
        void SetColor(Color color);
        void SetLineWidth(float w);
        void DrawLine(Vector2 o, Vector2 p);
        void Begin();
        void End();
        void Clear();
    }
}