using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using YooAsset;

namespace Test
{
    public class Test_SpriteSheet : MonoBehaviour
    {
        public Image image;
        private string packageName = "DefaultPackage";
        private ResourcePackage package;
        // Start is called before the first frame update
        IEnumerator Start()
        {
            YooAssets.Initialize();
            // 创建资源包裹类
            package = YooAssets.TryGetPackage(packageName);
            if (package == null)
            {
                package = YooAssets.CreatePackage(packageName);
            }
            var createParameters = new OfflinePlayModeParameters();
            createParameters.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
            var initializationOperation = package.InitializeAsync(createParameters);
            yield return initializationOperation;

            var operation = package.RequestPackageVersionAsync();
            yield return operation;

            var operation1 = package.UpdatePackageManifestAsync(operation.PackageVersion);
            yield return operation1;

            var sprite = package.LoadAssetAsync<Sprite>("Assets/Test/adv_bg");
            yield return sprite;
            image.sprite = sprite.AssetObject as Sprite;
            image.SetNativeSize();

            // var sprite = Resources.LoadAsync<Sprite>("adv_bg");
            // yield return sprite;
            // // image.sprite = sprite.asset as Sprite;
            // image.SetNativeSize();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
