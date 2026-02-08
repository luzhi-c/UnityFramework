using GameFramework;
using UnityEngine;

namespace GamePlay.Game
{
    public class Rect
    {
        /// <summary>
        /// 矩形左上角基于原点_cx的偏移x
        /// </summary>
        private float _x;

        public float x
        {
            get => _x;
            set => _x = value;
        }
        /// <summary>
        /// 矩形左上角基于原点_cy的偏移y
        /// </summary>
        private float _y;
        public float y
        {
            get => _y;
            set => _y = value;
        }
        /// <summary>
        /// 宽度
        /// </summary>
        private float _w;
        /// <summary>
        /// 高度
        /// </summary>
        private float _h;
        /// <summary>
        /// 旋转角度
        /// </summary>
        private float _r;
        #region 转换作用
        /// <summary>
        /// 中心原点x
        /// </summary>
        private float _cx;
        /// <summary>
        /// 中心原点y
        /// </summary>
        private float _cy;
        /// <summary>
        /// 矩形最大右下角x
        /// </summary>
        private float _xw;
        public float xw
        {
            get => _xw;
            set => _xw = value;
        }
        /// <summary>
        /// 矩形最大右下角y
        /// </summary>
        private float _yh;
        public float yh
        {
            get => _yh;
            set => _yh = value;
        }
        #endregion 转换作用
        // 旋转
        Vector2 RotatePoint(float px, float py, float ox, float oy, float radian)
        {
            var x = ox - px;
            var y = py - oy;
            var cos = Mathf.Cos(radian);
            var sin = Mathf.Sin(radian);
            return new Vector2(x * cos - y * sin + ox, x * sin + y * cos + oy);
        }

        public Vector2 Rotate(float x, float y)
        {
            return RotatePoint(x, y, _cx, _cy, _r);
        }

        public void Set(float x, float y, float w, float h, float r, float cx, float cy)
        {
            _x = x;
            _y = y;
            _w = w;
            _h = h;
            _r = r;
            _cx = cx;
            _cy = cy;
            Adjust();
        }

        void Adjust()
        {
            _xw = _x + _w;
            _yh = _y + _h;
        }

        public bool CheckPoint(float x, float y)
        {
            var nx = x;
            var ny = y;
            if (_r != 0)
            {
                var r = Rotate(x, y);
                nx = r.x;
                ny = r.y;
            }
            if (nx < _x || nx > _xw || ny < _y || ny > _yh)
            {
                return false;
            }
            return true;
        }

        public ColliderResult CheckRect(Rect rect)
        {
            var lx = Mathf.Max(_x, rect.x);
            var ly = Mathf.Max(_y, rect.y);
            var rx = Mathf.Min(_xw, rect.xw);
            var ry = Mathf.Min(_yh, rect.yh);
            if (lx > rx || ly > ry)
            {
                return ColliderResult.Create(false);
            }
            return ColliderResult.Create(true, lx + (rx - lx) * 0.5f, ly + (ry - ly) * 0.5f);
        }

        /// <summary>
        /// 逻辑上原点在左上方 逻辑y轴正方向是向下的 但是渲染时向上才是y轴正方向
        /// </summary>
        /// <param name="gl"></param>
        /// <param name="color"></param>
        public void OnDraw(IGL gl, Color color)
        {
            gl.SetColor(color);
            var pointList = new Vector2[4] { new Vector2(_x, -y), new Vector2(_x, -yh), new Vector2(_xw, -_yh), new Vector2(_xw, -_y) };
            for (int i = 0; i < 4; i++)
            {
                var startPoint = pointList[i];
                var endPoint = pointList[(i + 1) % 4];
                gl.DrawLine(startPoint, endPoint);
            }
        }

    }
}