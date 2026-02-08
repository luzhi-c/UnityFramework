using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.IO;
using ExtractorSharp.Core.Lib;
using Ionic.Zlib;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using YooAsset;

namespace Test
{
    public class Test_NPK : MonoBehaviour
    {
        string packageName = "DefaultPackage";
        ResourcePackage package;
        string npkName = "sprite_character_swordman_equipment_avatar_skin.NPK";
        string fileName = "sm_body80100.img";
        // Start is called before the first frame update

        public SpriteRenderer sp;
        public RawImage rawImage;

        private ExtractorSharp.Core.Model.Album album;
        private Dictionary<int, Sprite> dic = new();
        private int[] offset = new int[] { 232, 333 };
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
            YooassetResourceManager.Instance.LoadAssetAsync<TextAsset>(npkName, (asset) =>
            {
                if (asset != null)
                {
                    try
                    {
                        var files = ExtractorSharp.Core.Coder.NpkCoder.ReadNpk(new MemoryStream(asset.bytes));
                        album = files.Find((v) => v.Name == fileName);
                        if (album != null)
                        {
                            play = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }

                }

            });

        }

        private float current = 0f;
        public float interval = 0.05f;
        private int index = 0;
        public bool play = false;
        // Update is called once per frame
        void Update()
        {
            if (!play)
            {
                return;
            }
            var dt = Time.deltaTime;
            current += dt;
            if (current >= interval)
            {
                current = 0f;
                index++;
                if (index >= album.Count)
                {
                    index = 0;
                }
                NextFrame();
            }
        }

        void NextFrame()
        {
            var t = album[index];
            if (!dic.TryGetValue(index, out var sprite))
            {
                var texture = ExtractorSharp.Core.Lib.Texture2D.FromArray(t.Picture.ToArray(), t.Picture.Size);
                float pivotX = 0.5f - (float)(t.X - offset[0] + t.Width / 2) / t.Width;
                float pivotY = 0.5f - (float)(-t.Y - t.Height / 2 + offset[1]) / t.Height;
                sprite = Sprite.Create(texture, new Rect(0, 0, t.Width, t.Height),
                new Vector2(pivotX, pivotY), 100);
                dic.Add(index, sprite);
            }
            sp.sprite = sprite;

            rawImage.texture = ExtractorSharp.Core.Lib.Texture2D.FromArray(t.Picture.ToArray(), t.Picture.Size);
            rawImage.SetNativeSize();

            rawImage.transform.localPosition = new(t.X - offset[0] + t.Width / 2, -t.Y - t.Height / 2 + offset[1], 0);
        }
    }
}

