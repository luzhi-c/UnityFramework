using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityGameFramework.Runtime;
using TMPro;

namespace Test
{
    public class TestTween : MonoBehaviour
    {
        public TextMeshProUGUI text;
        // Start is called before the first frame update
        void Start()
        {
            // var tween1 = transform.DOLocalMove(new Vector3(0, 0, 0), 0.5f).SetDelay(0.5f).SetLoops(3).OnComplete(() =>
            // {
            //     Debug.Log("1");
            // });
            // var tween2 = transform.DOLocalMove(new Vector3(0, 272, 0), 0.5f).Pause();
            #region 顺序播放
            // Sequence sequence = DOTween.Sequence();
            // sequence.Append(transform.DOLocalMoveY(0, 1f)); // 第一步
            // sequence.Append(transform.DORotate(new Vector3(0, 0, 90), 1f)); // 接上一步
            // sequence.Insert(0, transform.DOScale(3, 1f)); // 与第一步同时开始
            // sequence.AppendInterval(0.5f); // 延迟
            // sequence.SetLoops(-1, LoopType.Yoyo); // 循环
            // // sequence.Play();
            #endregion
            // transform.DOLocalJump(new Vector3(300, 200, 0), 10f, 1, 1f);
            #region 结束Tween
            // transform.DOKill();
            // DOTween.Kill(this);
            #endregion
            #region 同步播放
            // Sequence seq = DOTween.Sequence();
            // seq.Append(transform.DOScale(1.5f, 1f))
            //    .Join(transform.DORotate(new Vector3(0, 0, 90), 1f))
            //    .Append(transform.DOScale(1f, 1f))
            //    .Join(transform.DORotate(Vector3.zero, 1f))
            //    .SetLoops(-1).SetTarget(this).Play();
            #endregion

            // this.DOKill();

            #region 数值
            // DOTween.To(() => currentValue, x => currentValue = x, targetValue, 1f);
            // 常用于自定义变量动画
            int health = 300;
            DOTween.To(() => health, x => health = x, 0, 2f).OnUpdate(() =>
            {
                text.text = health + "";
            });
            #endregion


            #region path
            // Vector3[] path = new Vector3[] { new Vector3(300, 0, 0), new Vector3(0, 300, 0), new Vector3(-300, 0, 0), new Vector3(0, 0, 0) };
            // transform.DOLocalPath(path, 3f, PathType.CatmullRom).SetLoops(-1); 
            #endregion
            #region 暂时不播放
            // var tween2 = transform.DOLocalMove(new Vector3(0, 272, 0), 0.5f).Pause();
            #endregion
        }


        private float time = 5f;
        private bool pause = false;
        // Update is called once per frame
        void Update()
        {
            var dt = Time.deltaTime;
            if (!pause)
            {
                time -= dt;
                if (time <= 0)
                {
                    pause = true;
                    this.DOKill();
                }
            }
        }
    }

}
