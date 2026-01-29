using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;

public class Test_Spine : MonoBehaviour
{
    public SkeletonDataAsset skeletonDataAsset;
    [SpineAnimation(dataField: "skeletonDataAsset")]
    public string animationName;
    // Start is called before the first frame update



    public TextAsset skeletonJson;
    public TextAsset atlasText;
    public Texture2D[] textures;
    public Material materialPropertySource;
    SpineAtlasAsset runtimeAtlasAsset;
    SkeletonDataAsset runtimeSkeletonDataAsset;
    SkeletonAnimation runtimeSkeletonAnimation;

    IEnumerator Start()
    {
        // yield return Test1();
        yield return Test2();
    }

    IEnumerator Test1()
    {
        if (skeletonDataAsset == null) yield break;
        skeletonDataAsset.GetSkeletonData(false); // Preload SkeletonDataAsset.
        yield return null;
        var spineAnimation = skeletonDataAsset.GetSkeletonData(false).FindAnimation(animationName);
        var count = 20;
        for (int i = 0; i < count; i++)
        {
            var sa = SkeletonAnimation.NewSkeletonAnimationGameObject(skeletonDataAsset); // Spawn a new SkeletonAnimation GameObject.
            DoExtraStuff(sa, spineAnimation); // optional stuff for fun.
            sa.gameObject.name = i.ToString();
            yield return new WaitForSeconds(1f / 8f);
        }
    }

    IEnumerator Test2()
    {
        CreateRuntimeAssetsAndGameObject();
        runtimeSkeletonDataAsset.GetSkeletonData(false); // preload.
        yield return new WaitForSeconds(0.5f);

        runtimeSkeletonAnimation = SkeletonAnimation.NewSkeletonAnimationGameObject(runtimeSkeletonDataAsset);

        // Extra Stuff
        runtimeSkeletonAnimation.Initialize(false);
        runtimeSkeletonAnimation.Skeleton.SetSkin("weapon/sword");
        runtimeSkeletonAnimation.Skeleton.SetSlotsToSetupPose();
        runtimeSkeletonAnimation.AnimationState.SetAnimation(0, "run", true);
        runtimeSkeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = 10;
        runtimeSkeletonAnimation.transform.Translate(Vector3.down * 2);
    }

    void CreateRuntimeAssetsAndGameObject()
    {
        // 1. Create the AtlasAsset (needs atlas text asset and textures, and materials/shader);
        // 2. Create SkeletonDataAsset (needs json or binary asset file, and an AtlasAsset)
        // 3. Create SkeletonAnimation (needs a valid SkeletonDataAsset)

        runtimeAtlasAsset = SpineAtlasAsset.CreateRuntimeInstance(atlasText, textures, materialPropertySource, true);
        runtimeSkeletonDataAsset = SkeletonDataAsset.CreateRuntimeInstance(skeletonJson, runtimeAtlasAsset, true);
    }


    void DoExtraStuff(SkeletonAnimation sa, Spine.Animation spineAnimation)
    {
        sa.transform.localPosition = Random.insideUnitCircle * 6f;
        sa.transform.SetParent(transform, false);

        if (spineAnimation != null)
        {
            sa.Initialize(false);
            sa.AnimationState.SetAnimation(0, spineAnimation, true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
