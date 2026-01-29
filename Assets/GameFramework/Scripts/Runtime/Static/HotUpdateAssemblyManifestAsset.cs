using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HotUpdateAssemblyManifest", menuName = "ScriptableObject/HotUpdateAssemblyManifest")]
public class HotUpdateAssemblyManifestAsset : ScriptableObject
{
    public HotUpdateAssemblyManifestData data;
}


[Serializable]
public class HotUpdateAssemblyManifestData
{

    public const string DLLSuffix = ".dll";

    [Header("热更程序集")]
    public List<string> HotfixDlls;


    [Header("AOT 补充元数据dll列表")]
    public List<string> AOTMetadataDlls;

}
