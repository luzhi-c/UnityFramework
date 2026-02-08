using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GamePlay.Game;
using TMPro;
using UnityEngine;

public class DFQ_Collider : MonoBehaviour
{
    public GamePlay.Game.Collider A;
    public GamePlay.Game.Collider B;

    public RealRadarChart GL;

    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        A.SetAttack(new GamePlay.Game.SolidRectData[] { new GamePlay.Game.SolidRectData() { x = -0.17f, y1 = -0.05f, z = 0f, w = 0.3f, y2 = 0.1f, h = 0.97f }, new GamePlay.Game.SolidRectData() { x = 0.13f, y1 = -0.05f, z = 0f, w = 0.13f, y2 = 0.1f, h = 0.73f } });
        B.SetDamage(new GamePlay.Game.SolidRectData[] { new GamePlay.Game.SolidRectData() { x = -0.17f, y1 = -0.05f, z = 0f, w = 0.3f, y2 = 0.1f, h = 0.97f }, new GamePlay.Game.SolidRectData() { x = 0.13f, y1 = -0.05f, z = 0f, w = 0.13f, y2 = 0.1f, h = 0.73f } });
    }

    // Update is called once per frame
    void Update()
    {
        var a = A.transform.position;
        A.Set(a.x, -a.y, a.z, 1, 1, 0);

        var b = B.transform.position;
        B.Set(b.x, -b.y, b.z, 1, 1, 0);
        GL.Clear();
        GL.SetLineWidth(0.02f);
        A.OnDraw(GL);
        B.OnDraw(GL);
        GL.End();

        if (text)
        {
            var result = A.Collide(B);
            if (result.IsHit)
            {
                text.text = "击中";
            }
            else
            {
                text.text = "未击中";
            }

            ReferencePool.Release(result);
        }
    }
}
