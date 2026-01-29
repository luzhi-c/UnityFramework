using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using GameFramework;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using YooAsset;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GamePlay.Boot
{
    public class ProcedureStartGame : ProcedureBase
    {

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Debug(Utility.Text.Format("进入流程: {0}", GetType().ToString()));
            var package = YooAssets.GetPackage(GameStatic.DefaultPackage);
            var assetHandle = package.LoadAssetSync<HotUpdateAssemblyManifestAsset>("Assets/GameSetting/HotUpdateAssemblyManifest");
            var mainfest = assetHandle.AssetObject as HotUpdateAssemblyManifestAsset;
            foreach (var item in mainfest.data.HotfixDlls)
            {
                var asset = package.LoadAssetSync<TextAsset>("Assets/HotUpdateDlls/" + item + HotUpdateAssemblyManifestData.DLLSuffix).AssetObject as TextAsset;
                Assembly.Load(asset.bytes);
            }
            foreach (var item in mainfest.data.AOTMetadataDlls)
            {
                var asset = package.LoadAssetSync<TextAsset>("Assets/AOTDlls/" + item + HotUpdateAssemblyManifestData.DLLSuffix).AssetObject as TextAsset;
                HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(asset.bytes, HybridCLR.HomologousImageMode.SuperSet);
            }
            YooAssets.LoadSceneAsync("GameMain");
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }
    }
}