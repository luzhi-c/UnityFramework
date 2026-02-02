using System;
using UnityEngine;
using UnityEngine.UI;
namespace Test
{
    public class Test_GuideLayer : MonoBehaviour
    {
        public Canvas canvas;
        public GameObject go1;
        public GameObject go2;

        private Material material;

        private Vector4 center;
        private Vector3[] corners = new Vector3[4];
        private float radiu = 0f;

        public GuideMaskController guideMaskController;
        public void Awake()
        {
            material = GetComponent<Image>().material;
            SetCenter(go1);
        }

        private float current = 0f;
        void SetCenter(GameObject go)
        {
            // 这个方法返回四个角 由左下角顺时针到右下角  左下角->左上角->右上角->右下角
            var rectTransform = go.transform as RectTransform;
            rectTransform.GetWorldCorners(corners);
            radiu = Vector2.Distance(WorldToCanvasPos(canvas, corners[0]), WorldToCanvasPos(canvas, corners[2])) / 2f;

            float x = corners[0].x + (corners[3].x - corners[0].x) / 2f;
            float y = corners[0].y + (corners[1].y - corners[0].y) / 2f;
            var center = new Vector3(x, y, 0);
            var pos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, center, canvas.GetComponent<Camera>(), out pos);

            this.center = new Vector4(pos.x, pos.y, 0, 0);

            material.SetVector("_Center", this.center);


            (canvas.transform as RectTransform).GetWorldCorners(corners);
            for (int i = 0; i < corners.Length; i++)
            {
                current = Mathf.Max(Vector3.Distance(WorldToCanvasPos(canvas, corners[i]), this.center), current);
            }

            guideMaskController.SetTarget(go);
        }

        Vector2 WorldToCanvasPos(Canvas canvas, Vector3 world)
        {
            Vector2 pos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, world, canvas.GetComponent<Camera>(), out pos);
            return pos;
        }

        private float tempVe = 0f;
        public void Update()
        {
            float value = Mathf.SmoothDamp(current, radiu, ref tempVe, 0.3f);
            if (!Mathf.Approximately(current, radiu))
            {
                current = value;
                material.SetFloat("_Radiu", value);
            }
        }
    }
}
