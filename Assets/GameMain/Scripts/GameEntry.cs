using System.Collections;
using System.Collections.Generic;
using cfg;
using GameFramework;
using GamePlay.Boot;
using Luban;
using TMPro;
using UnityEngine;
using UnityGameFramework.Runtime;
using YooAsset;
namespace GamePlay.Game
{
    public class GameEntry : MonoBehaviour
    {
        public TextMeshProUGUI text;
        private string m_BasePath = "Assets/Config/LubanData/{0}";
        private string[] m_Cfg = { "tbitem", "tbnewitem" };
        private int m_CompleteCount = 0;
        private Dictionary<string, TextAsset> m_CfgMap = new();
        // Start is called before the first frame update
        void Start()
        {
            var package = YooAssets.GetPackage(GameStatic.DefaultPackage);

            for (int i = 0; i < m_Cfg.Length; i++)
            {
                var cfgName = m_Cfg[i];
                var handler = package.LoadAssetAsync<TextAsset>(Utility.Text.Format(m_BasePath, cfgName));
                handler.Completed += (AssetHandle po) =>
                {
                    if (po.IsDone)
                    {
                        LoadComplete(cfgName, po.AssetObject as TextAsset);
                    }
                };
            }

        }

        void LoadComplete(string cfgName, TextAsset asset)
        {
            m_CompleteCount++;
            m_CfgMap.Add(cfgName, asset);
            if (m_CompleteCount >= m_Cfg.Length)
            {
                var tables = new Tables(Loader);
                var txt = tables.TbItem.Get(1002).Desc;
                // YooAssets.LoadSceneAsync("Guide");
                text.text = txt;

            }

        }

        ByteBuf Loader(string name)
        {
            return new ByteBuf(m_CfgMap[name].bytes);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
