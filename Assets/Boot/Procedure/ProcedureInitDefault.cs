using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using YooAsset;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GamePlay.Boot
{
    public class ProcedureInitDefault : ProcedureBase
    {
        private PatchOperation op;
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Debug(Utility.Text.Format("进入流程: {0}", GetType().ToString()));

            YooAssets.Initialize();
            op = new PatchOperation(GameStatic.DefaultPackage, EPlayMode.OfflinePlayMode, false);
            YooAssets.StartOperation(op);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (op.IsDone)
            {
                var gamePackage = YooAssets.GetPackage(GameStatic.DefaultPackage);
                YooAssets.SetDefaultPackage(gamePackage);
                ChangeState<ProcedureStartGame>(procedureOwner);
            }
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