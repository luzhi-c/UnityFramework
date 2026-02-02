using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using YooAsset;

namespace Test
{
    public class Test_UI : MonoBehaviour
    {
        public Button button1;
        public Button button2;
        public Button button3;
        string packageName = "DefaultPackage";
        ResourcePackage package;
        public Canvas canvas;
        // Start is called before the first frame update

        void Awake()
        {
            UIManager.Instance.Init(canvas.transform);
            UIManager.Instance.RegisterUIInfo(new UIViewInfo() { AssetPath = "Assets/Test/UI/Panel1", Layer = UILayer.VIEW, Name = "Panel1" });
            UIManager.Instance.RegisterUIInfo(new UIViewInfo() { AssetPath = "Assets/Test/UI/Panel2", Layer = UILayer.VIEW, Name = "Panel1" });
            UIManager.Instance.RegisterUIInfo(new UIViewInfo() { AssetPath = "Assets/Test/UI/Popup1", Layer = UILayer.POPUP, Name = "Popup1" });
            UIManager.Instance.RegisterUIInfo(new UIViewInfo() { AssetPath = "Assets/Test/UI/Popup2", Layer = UILayer.POPUP, Name = "Popup2" });

            button1.onClick.AddListener(() =>
            {
                UIManager.Instance.OpenUI("Panel1", null);
            });
            button2.onClick.AddListener(() =>
           {
               UIManager.Instance.OpenUI("Popup1", "1");
           });
            button3.onClick.AddListener(() =>
           {
               UIManager.Instance.OpenUI("Popup2", "2");
           });
        }
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

            YooassetResourceManager.Instance.AddPackage(packageName, package);

            // UIManager.Instance.OpenUI("Panel1", null);
            // UIManager.Instance.OpenUI("Popup1", null);
            // UIManager.Instance.OpenUI("Popup2", null);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
