using UnityEngine;
using YooAsset;
using System.Linq;
using HybridCLR;
using System.Reflection;
using UnityGameFramework.Runtime;

namespace GamePlay.Boot
{

    public partial class GameEntry : MonoBehaviour
    {
        public static MonoBehaviour Instance;
        void Start()
        {
            Instance = this;
            InitBuiltinComponents();
            DontDestroyOnLoad(gameObject);
        }
    }
}
