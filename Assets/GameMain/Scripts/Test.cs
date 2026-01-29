using System.Collections;
using System.Collections.Generic;
using cfg;
using GameFramework;
using GamePlay.Boot;
using Luban;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using YooAsset;
namespace GamePlay.Game
{
    public class Test : MonoBehaviour
    {
        public Image image;
        // Start is called before the first frame update
        void Start()
        {
            var package = YooAssets.GetPackage(GameStatic.DefaultPackage);

            var h = package.LoadAssetAsync<Sprite>("Assets/Res/PatchView/login");
            h.Completed += (AssetHandle ah) =>
            {
                image.sprite = ah.AssetObject as Sprite;
            };
        }
    }
}
