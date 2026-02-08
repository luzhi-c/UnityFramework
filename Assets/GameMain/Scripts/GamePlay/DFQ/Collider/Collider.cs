using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace GamePlay.Game
{
    public class Collider : MonoBehaviour
    {
        /// <summary>
        /// 受击框
        /// </summary>
        private List<SolidRect> m_Damage = new();

        public List<SolidRect> Damage
        {
            get { return m_Damage; }
        }
        /// <summary>
        /// 攻击框
        /// </summary>
        private List<SolidRect> m_Attack = new();
        public List<SolidRect> Attack
        {
            get { return m_Attack; }
        }
        public void SetAttack(SolidRectData[] data)
        {
            m_Attack.Clear();
            for (int i = 0; i < data.Length; i++)
            {
                m_Attack.Add(new SolidRect(data[i]));
            }
        }

        public void SetDamage(SolidRectData[] data)
        {
            m_Damage.Clear();
            for (int i = 0; i < data.Length; i++)
            {
                m_Damage.Add(new SolidRect(data[i]));
            }
        }

        public void Set(float px, float py, float pz, float sx, float sy, float r)
        {
            for (int i = 0; i < m_Attack.Count; i++)
            {
                m_Attack[i].Set(px, py, pz, sx, sy, r);
            }
            for (int i = 0; i < m_Damage.Count; i++)
            {
                m_Damage[i].Set(px, py, pz, sx, sy, r);
            }
        }

        public ColliderResult Collide(Collider opponent)
        {
            var result = ColliderResult.Create(false);
            var a = Attack;
            var b = opponent.Damage;
            if (a.Count <= 0 || b.Count <= 0)
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
                        ReferencePool.Release(result);
                        return tempResult;
                    }
                    ReferencePool.Release(tempResult);
                }
            }
            return result;
        }
        public void OnDraw(IGL gl)
        {
            for (int i = 0; i < m_Attack.Count; i++)
            {
                m_Attack[i].OnDraw(gl);
            }
            for (int i = 0; i < m_Damage.Count; i++)
            {
                m_Damage[i].OnDraw(gl);
            }
        }

    }
}