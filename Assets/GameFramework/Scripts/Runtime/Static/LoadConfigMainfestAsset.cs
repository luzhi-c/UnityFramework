using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoadConfigMainfest", menuName = "ScriptableObject/LoadConfigMainfest")]
public class LoadConfigMainfestAsset : ScriptableObject
{
    public LoadConfigMainfestData data;
}


[Serializable]
public class LoadConfigMainfestData
{

    [Header("配置集")]
    public List<string> configs;

}
