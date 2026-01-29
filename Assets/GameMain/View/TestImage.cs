using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class TestImage : MonoBehaviour
{
    private int Index = 0;
    private int maxIndex = 10;

    private float changeDelay = 1f;
    private float currentDelay = 0;
    // Start is called before the first frame update

    private ResourcePackage gamePackage;
    private Image image;
    void Start()
    {
        gamePackage = YooAssets.GetPackage("DefaultPackage");
        image = GetComponent<Image>();
        SetImage(0);
    }

    void SetImage(int index)
    {
        var s = gamePackage.LoadAssetAsync<Sprite>("Assets/Res/Tree/" + index);
        s.Completed += (AssetHandle sp) =>
        {
            image.sprite = sp.AssetObject as Sprite;
            image.SetNativeSize();
            s.Release();
        };
    }

    // Update is called once per frame
    void Update()
    {
        var dt = Time.deltaTime;
        currentDelay += dt;
        if (currentDelay >= changeDelay)
        {
            currentDelay = 0;
            Index++;
            if (Index > maxIndex)
            {
                Index = 0;
            }
            SetImage(Index);
        }


    }
}
