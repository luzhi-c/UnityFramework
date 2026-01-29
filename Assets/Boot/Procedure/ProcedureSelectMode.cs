using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Procedure;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GamePlay.Boot
{
    enum ESelectMode
    {
        Single,
        HotPlay,
    }
    public class ProcedureSelectMode : ProcedureBase
    {
        private bool m_SelectComplete = false;
        private ESelectMode m_PlayMode = ESelectMode.Single;
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Debug(Utility.Text.Format("进入流程: {0}", GetType().ToString()));

            var canvas = GameObject.Find("Canvas");

            var proBar = canvas.transform.Find("m_ProBar");
            proBar.gameObject.SetActive(false);

            var desc = canvas.transform.Find("m_Desc");
            desc.gameObject.SetActive(false);

            var proNum = canvas.transform.Find("m_ProNum");
            proNum.gameObject.SetActive(false);

            var btn1 = canvas.transform.Find("m_Btn1").GetComponent<Button>();
            var btn2 = canvas.transform.Find("m_Btn2").GetComponent<Button>();

            btn1.onClick.AddListener(() =>
            {
                m_SelectComplete = true;
                m_PlayMode = ESelectMode.Single;

                HideBtns();
            });

            btn2.onClick.AddListener(() =>
          {
              m_SelectComplete = true;
              m_PlayMode = ESelectMode.HotPlay;
              HideBtns();
          });
        }

        void HideBtns()
        {
            var canvas = GameObject.Find("Canvas");
            var btn1 = canvas.transform.Find("m_Btn1");
            var btn2 = canvas.transform.Find("m_Btn2");
            btn1.gameObject.SetActive(false);
            btn2.gameObject.SetActive(false);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (m_SelectComplete)
            {
                if (m_PlayMode == ESelectMode.Single)
                {
                    ChangeState<ProcedureInitDefault>(procedureOwner);
                }
                else if (m_PlayMode == ESelectMode.HotPlay)
                {
                    ChangeState<ProcedureCheckVersion>(procedureOwner);
                }
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