using System.Collections;
using System.Collections.Generic;
using GamePlay.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class DFQ_Transform : MonoBehaviour
    {

        public Button button1;
        public Button button2;

        public Button button3;
        public Button button4;

        public TransformComponent trans;
        public InputComponent input;

        public float speed = 5f;

        private string[] arrows = new string[] { "left", "right", "up", "down" };
        private Vector3[] arrowVector = new Vector3[] { Vector3.left, Vector3.right, Vector3.up, Vector3.down };
        // Start is called before the first frame update
        void Start()
        {
            trans.position = trans.transform.localPosition;

            button1.onClick.AddListener(() =>
            {
                OnClick("left");
            });
            button2.onClick.AddListener(() =>
            {
                OnClick("right");
            });

            button3.onClick.AddListener(() =>
           {
               OnClick("up");
           });
            button4.onClick.AddListener(() =>
            {
                OnClick("down");
            });
        }

        void OnClick(string dir)
        {
            if (!input)
            {
                return;
            }
            if (input.IsHold(dir))
            {
                input.OnReleased(dir);
            }
            else
            {
                input.OnPressed(dir);
            }
        }

        // Update is called once per frame
        void Update()
        {
            var dt = Time.deltaTime;
            if (input)
            {
                input.OnUpdate(dt);
                for (int i = 0; i < arrows.Length; i++)
                {
                    if (input.IsHold(arrows[i]))
                    {
                        trans.positionTick = true;
                        trans.position += arrowVector[i] * speed * dt;
                    }
                }
            }
            if (trans)
            {
                trans.OnUpdate(dt);
                trans.OnLateUpdate(dt);
            }
        }
    }
}

