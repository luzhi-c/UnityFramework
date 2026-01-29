using System;
using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GamePlay.Boot
{
    public class ProcedureCheckVersion : ProcedureBase
    {
        private bool m_CheckVersionComplete = false;
        private bool m_NeedUpdateVersion = false;
        private string checkVersionUrl = "https://livetest-global.topjoy.com/live/login";
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Debug(Utility.Text.Format("进入流程: {0}", GetType().ToString()));
            // WWWForm.data = data;
            // 向服务器请求版本信息
            var dd = new CheckeVersionData()
            {
                roomId = "1",
                userId = "1",
                appId = "ks652740473287089891",
                clientVersion = "1.0.0",
                token = "",
                debugtest = true
            };
            var data = Utility.Json.ToJson(dd);
            // GameEntry.WebRequest.AddWebRequest(checkVersionUrl, Utility.Converter.GetBytes(data), this);

            WebRequestExtra.Request(checkVersionUrl, data, OnWebRequestSuccess, OnWebRequestFailure);
            var canvas = GameObject.Find("Canvas");
            var desc = canvas.transform.Find("m_Desc").GetComponent<TextMeshProUGUI>();
            desc.gameObject.SetActive(true);
            desc.text = "请求远程版本...";

            // m_CheckVersionComplete = true;
            // m_NeedUpdateVersion = true;
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (!m_CheckVersionComplete)
            {
                return;
            }

            if (m_NeedUpdateVersion)
            {
                ChangeState<ProcedureHotUpdate>(procedureOwner);
            }
            else
            {
                ChangeState<ProcedureInitDefault>(procedureOwner);
            }
        }

        private void GotoUpdateApp(object userData)
        {
            var url = "";
            if (!string.IsNullOrEmpty(url))
            {
                Application.OpenURL(url);
            }
        }

        private void OnWebRequestSuccess(string str)
        {
            m_CheckVersionComplete = true;
            m_NeedUpdateVersion = true;
        }

        private void OnWebRequestFailure()
        {
            Log.Warning("Check version failure");
            m_CheckVersionComplete = true;
            m_NeedUpdateVersion = false;
        }
    }
}
