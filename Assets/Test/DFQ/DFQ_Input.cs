using GameFramework;
using GamePlay.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class DFQ_Input : MonoBehaviour
    {
        public Button button1;
        public Button button2;
        public TextMeshProUGUI text;
        public InputComponent input;

        public string key = "Click";
        // Start is called before the first frame update
        void Start()
        {
            button1.onClick.AddListener(() =>
            {
                input.OnPressed(key);
            });
            button2.onClick.AddListener(() =>
            {
                input.OnReleased(key);
            });
        }

        // Update is called once per frame
        void Update()
        {
            if (input)
            {
                input.OnUpdate(Time.deltaTime);

                text.text = Utility.Text.Format("状态：{0}", input.Get(key));
            }
        }

    }
}

