using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace GamePlay.Game
{

    public struct SolidRectData
    {
        public float x;
        public float y1;
        public float y2;
        public float z;
        public float w;
        public float h;
    }

    public struct RectGroup
    {
        public Rect xy;
        public Rect xz;
    }

    public class SolidRect
    {
        private float _x = 0;
        private float _y = 0;
        private float _z = 0;
        private float _sx = 1;
        private float _sy = 1;
        private float _r = 0;
        private SolidRectData _struct;
        private RectGroup _rectGroup;

        public SolidRect(float x, float y1, float y2, float z, float w, float h)
        {
            _struct = new SolidRectData() { x = x, y1 = y1, y2 = y2, z = z, w = w, h = h };
            _rectGroup = new RectGroup() { xy = new Rect(), xz = new Rect() };
        }

        public SolidRect(SolidRectData data)
        {
            _struct = data;
            _rectGroup = new RectGroup() { xy = new Rect(), xz = new Rect() };
        }

        public void Set(float x, float y, float z, float sx, float sy, float r)
        {
            _x = x;
            _y = y;
            _z = z;
            _sx = sx;
            _sy = sy;
            _r = r;
            Adjust();
        }

        void Adjust()
        {
            var x = _x + _struct.x * _sx;
            var w = _struct.w * Mathf.Abs(_sx);
            var y1 = _y + _struct.y1 * _sy;
            var h1 = (_struct.y2 - _struct.y1) * Mathf.Abs(_sy);
            var y2 = _y + _z + (-_struct.z - _struct.h) * Mathf.Abs(_sy);
            var h2 = _struct.h * Mathf.Abs(_sy);
            if (_sx < 0)
            {
                x = x - w;
            }

            if (_sy < 0)
            {
                y1 = y1 - h1;
                y2 = y2 - h2;
            }

            _rectGroup.xy.Set(x, y1, w, h1, _r, _x, _y);
            _rectGroup.xz.Set(x, y2, w, h2, _r, _x, _y);
        }

        public ColliderResult Collide(SolidRect solidRect)
        {
            var result = ColliderResult.Create(false);
            var resultXY = _rectGroup.xy.CheckRect(solidRect._rectGroup.xy);
            var resultXZ = _rectGroup.xz.CheckRect(solidRect._rectGroup.xz);
            if (resultXY.IsHit && resultXZ.IsHit)
            {
                result.IsHit = true;
                result.X = resultXZ.X;
                result.Y = _y;
                result.Z = resultXZ.Y - _y;
            }
            ReferencePool.Release(resultXY);
            ReferencePool.Release(resultXZ);
            return result;
        }

        public bool CheckPoint(float x, float y)
        {
            return _rectGroup.xy.CheckPoint(x, y);
        }

        public static ColliderResult CollideWithList(List<SolidRect> a, List<SolidRect> b)
        {
            var result = ColliderResult.Create(false);
            if (a == null || b == null)
            {
                return result;
            }
            for (int i = 0; i < a.Count; i++)
            {
                for (int j = 0; j < b.Count; j++)
                {
                    var tempResult = a[i].Collide(b[j]);
                    if (tempResult.IsHit)
                    {
                        result.IsHit = true;
                        result.X = tempResult.X;
                        result.Y = tempResult.Y;
                        result.Z = tempResult.Z;
                        ReferencePool.Release(tempResult);
                        return result;
                    }
                }
            }
            return result;
        }

        public void OnDraw(IGL gl)
        {
            _rectGroup.xy.OnDraw(gl, Color.red);
            _rectGroup.xz.OnDraw(gl, Color.white);
        }
    }
}