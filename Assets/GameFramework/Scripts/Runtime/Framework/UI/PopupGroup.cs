using System.Collections.Generic;
using System.Linq;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityEngine.UI;

namespace UnityGameFramework.Runtime
{
    public class PopupGroup
    {
        private UIManager m_UIManager;
        private Transform m_TouchLayer;
        private List<PopupInfo> m_PopupViewStack = new List<PopupInfo>();
        private Dictionary<int, PopupViewMediator> m_PopViewMap = new Dictionary<int, PopupViewMediator>();
        private Dictionary<string, PopupViewMediator> m_KeepPopupInstMap = new Dictionary<string, PopupViewMediator>();

        public PopupGroup(UIManager manager)
        {
            m_UIManager = manager;
            var touchLayer = GameObject.Instantiate(Resources.Load<GameObject>("UI/Mask"));
            m_TouchLayer = touchLayer.transform;
            m_TouchLayer.SetParent(m_UIManager.GetUIGroup(UILayer.POPUP), false);
            touchLayer.GetComponent<Button>().onClick.AddListener(onTouchMaskClick);
            touchLayer.SetActive(false);
        }

        public void AddPopupView(PopupViewMediator mediator, OpenUIFormInfo openUIFormInfo)
        {
            var popupInfo = PopupInfo.Create(openUIFormInfo.UIFormName, mediator, m_PopupViewStack.Count + 1, null);
            m_PopupViewStack.Add(popupInfo);
            CheckTouchLayer();
            UpdatePopupSiblingIndex();

            m_PopViewMap.Add(mediator.SerialId, mediator);
        }

        public bool HasPopup(int serialId)
        {
            return m_PopViewMap.ContainsKey(serialId);
        }

        private void ResumePopupView()
        {
            if (m_PopupViewStack.Count > 0)
            {
                var topPopInfo = m_PopupViewStack[m_PopupViewStack.Count - 1];
                var mediator = topPopInfo.Mediator;
                mediator.gameObject.SetActive(true);
                mediator.OnResume();
            }
            CheckTouchLayer();
        }

        public void RemovePopupView(PopupViewMediator mediator)
        {
            var topPopInfo = m_PopupViewStack[m_PopupViewStack.Count - 1];
            PopupInfo curPopInfo = null;
            for (int i = m_PopupViewStack.Count - 1; i >= 0; i--)
            {
                var topInfo = m_PopupViewStack[i];
                if (topInfo.Mediator == mediator)
                {
                    m_PopupViewStack.RemoveRange(i, 1);
                    curPopInfo = topInfo;
                    break;
                }
            }
            if (curPopInfo != null)
            {
                m_PopupViewStack.Remove(curPopInfo);
                ReferencePool.Release(curPopInfo);
                if (m_PopupViewStack.Count > 0)
                {
                    if (topPopInfo.Mediator == mediator)
                    {
                        ResumePopupView();
                    }
                }
                else
                {
                    CheckTouchLayer();
                }
            }

            m_UIManager.WillRemovePopupView(mediator, this, m_PopupViewStack.Count == 0);
        }

        void CheckTouchLayer()
        {
            if (m_PopupViewStack.Count <= 0)
            {
                m_TouchLayer.gameObject.SetActive(false);
            }
            else
            {
                m_TouchLayer.gameObject.SetActive(true);

                var topPopInfo = m_PopupViewStack[m_PopupViewStack.Count - 1];
                if (topPopInfo != null)
                {
                    var zOrder = topPopInfo.ZOrder - 1;
                    m_TouchLayer.SetSiblingIndex(zOrder);
                    UpdatePopupSiblingIndex();
                }
            }
        }

        void onTouchMaskClick()
        {
            if (m_PopupViewStack.Count > 0)
            {
                var topPopInfo = m_PopupViewStack[m_PopupViewStack.Count - 1];
                if (topPopInfo != null && topPopInfo.Mediator != null)
                {
                    topPopInfo.Mediator.OnTouchMaskLayer();
                }
            }
        }

        void UpdatePopupSiblingIndex()
        {
            for (int i = 0; i < m_PopupViewStack.Count; i++)
            {
                var zorder = i + 1;
                m_PopupViewStack[i].ZOrder = zorder;
                if (m_PopupViewStack[i].Mediator != null)
                {
                    m_PopupViewStack[i].Mediator.gameObject.transform.SetSiblingIndex(zorder);
                }
            }
        }

        void CleanPopup()
        {
            for (int i = m_PopupViewStack.Count - 1; i >= 0; i--)
            {
                var popupInfo = m_PopupViewStack[i];
                if (popupInfo.Mediator != null)
                {
                    m_UIManager.WillRemovePopupView(popupInfo.Mediator, this, m_PopupViewStack.Count == 0);
                    GameObject.Destroy(popupInfo.Mediator.gameObject);
                }
            }
            m_PopupViewStack.Clear();
            CheckTouchLayer();
        }

        public void SetActive(bool active)
        {
            for (int i = 0; i < m_PopupViewStack.Count; i++)
            {
                var mediator = m_PopupViewStack[i].Mediator;
                if (mediator != null)
                {
                    mediator.gameObject.SetActive(active);
                }
            }
            if (m_PopupViewStack.Count > 0 && m_TouchLayer != null)
            {
                m_TouchLayer.gameObject.SetActive(active);
            }
        }

        public void Dispose()
        {
            CleanPopup();
            if (m_TouchLayer != null)
            {
                GameObject.Destroy(m_TouchLayer.gameObject);
                m_TouchLayer = null;
            }
        }
    }
}