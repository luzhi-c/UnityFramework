using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public class UIManager : Singleton<UIManager>
    {
        private bool m_IsInit;
        private Transform m_InstanceRoot;
        private int m_Serial;
        private Dictionary<string, UIViewInfo> m_UIViewInfos = new Dictionary<string, UIViewInfo>();
        private readonly Dictionary<UILayer, Transform> m_UIGroups = new Dictionary<UILayer, Transform>();
        private readonly Dictionary<int, string> m_UIFormsBeingLoaded = new Dictionary<int, string>();
        private YooassetResourceManager m_ResourceManager;
        private Dictionary<int, BaseViewMediator> m_Views = new Dictionary<int, BaseViewMediator>();
        private List<AreaViewMediator> m_AreaViewList = new List<AreaViewMediator>();
        private Dictionary<string, BaseViewMediator> m_KeepViewInstanceMap = new Dictionary<string, BaseViewMediator>();
        private PopupGroup m_GlobalGroup;
        private Dictionary<AreaViewMediator, PopupGroup> m_PopupViewGroups = new Dictionary<AreaViewMediator, PopupGroup>();
        public UIManager()
        {

        }

        public void Init(Transform root)
        {
            m_IsInit = true;
            m_ResourceManager = YooassetResourceManager.Instance;
            m_InstanceRoot = root;
            AddUIGroup(UILayer.VIEW, 0);
            AddUIGroup(UILayer.POPUP, 1);
            AddUIGroup(UILayer.GUIDE, 2);
            AddUIGroup(UILayer.TIPS, 3);
            AddUIGroup(UILayer.SYSTEM, 4);
        }

        /// <summary>
        /// 增加界面组。
        /// </summary>
        /// <param name="uiGroupName">界面组名称。</param>
        /// <param name="depth">界面组深度。</param>
        /// <returns>是否增加界面组成功。</returns>
        public bool AddUIGroup(UILayer uiLayer, int depth)
        {
            if (!m_InstanceRoot)
            {
                Log.Error("UI Canvas is Invaild");
                return false;
            }
            if (HasUIGroup(uiLayer))
            {
                return false;
            }
            Transform uiGroup = new GameObject(Utility.Text.Format("UI Group - {0}", uiLayer)).transform;
            uiGroup.gameObject.layer = LayerMask.NameToLayer("UI");
            Transform transform = uiGroup.transform;
            transform.SetParent(m_InstanceRoot, false);
            // transform.SetSiblingIndex(depth);
            transform.localScale = Vector3.one;
            m_UIGroups.Add(uiLayer, uiGroup);
            return true;
        }

        public void RegisterUIInfo(UIViewInfo info)
        {
            if (m_UIViewInfos.ContainsKey(info.Name))
            {
                Log.Warning("界面{0}已注册", info.Name);
                return;
            }
            m_UIViewInfos.Add(info.Name, info);
        }

        public bool HasUIGroup(UILayer uiLayer)
        {
            return m_UIGroups.ContainsKey(uiLayer);
        }

        public Transform GetUIGroup(UILayer uiLayer)
        {
            if (HasUIGroup(uiLayer))
            {
                return m_UIGroups[uiLayer];
            }
            return null;
        }

        /// <summary>
        /// 是否存在界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <returns>是否存在界面。</returns>
        public bool HasUIForm(int serialId)
        {
            return true;
        }

        public int OpenUI(string uiFormName, object data)
        {
            var uiViewInfo = GetUIViewInfo(uiFormName);
            if (uiViewInfo == null)
            {
                Log.Warning("界面{0}未注册", uiFormName);
                return -1;
            }
            return OpenUIForm(uiViewInfo, data, true);
        }

        public UIViewInfo GetUIViewInfo(string uiFormName)
        {
            if (m_UIViewInfos.ContainsKey(uiFormName))
            {
                return m_UIViewInfos[uiFormName];
            }
            return null;
        }

        public int OpenUI(string viewName, object data, bool bind = true)
        {
            UIViewInfo uiViewInfo = GetUIViewInfo(viewName);
            if (uiViewInfo == null)
            {
                return -1;
            }
            return OpenUIForm(uiViewInfo, data, bind);
        }

        private int OpenUIForm(UIViewInfo uiViewInfo, object data, bool bind)
        {
            int serialId = ++m_Serial;
            m_UIFormsBeingLoaded.Add(serialId, uiViewInfo.Name);
            var openUIFormInfo = OpenUIFormInfo.Create(serialId, uiViewInfo.Name, data, bind);
            m_ResourceManager.LoadAssetAsync<GameObject>(uiViewInfo.AssetPath, (asset) => { OnLoadUIFormSuccess(asset, openUIFormInfo); }, () => { OnLoadUIFormFailure(openUIFormInfo); });
            return serialId;
        }

        public void InternalOpenUIForm(GameObject asset, OpenUIFormInfo openUIFormInfo)
        {
            UIViewInfo uiViewInfo = GetUIViewInfo(openUIFormInfo.UIFormName);
            var uiGroup = GetUIGroup(uiViewInfo.Layer);
            if (uiGroup == null)
            {
                return;
            }
            var viewInstance = GameObject.Instantiate(asset);
            viewInstance.transform.SetParent(uiGroup, false);

            BaseViewMediator mediator = viewInstance.GetComponent<BaseViewMediator>();

            if (mediator != null)
            {
                mediator.OnInit(openUIFormInfo.UserData);
                // 设置数据
                mediator.SerialId = openUIFormInfo.SerialId;
                // 加入管理
                m_Views.Add(openUIFormInfo.SerialId, mediator);
                if (uiViewInfo.Layer == UILayer.VIEW)
                {
                    m_AreaViewList.Add(mediator as AreaViewMediator);
                }
                else if (uiViewInfo.Layer == UILayer.POPUP)
                {
                    if (!(mediator is PopupViewMediator))
                    {
                        Log.Error("界面{0}上没有挂载PopupViewMediator组件,转化失败!", uiViewInfo.Name);
                        return;
                    }
                    if (m_AreaViewList.Count <= 0)
                    {
                        if (m_GlobalGroup == null)
                        {
                            m_GlobalGroup = new PopupGroup(this);
                        }
                        m_GlobalGroup.AddPopupView(mediator as PopupViewMediator, openUIFormInfo);
                    }
                    else
                    {
                        var topAreaViewInfo = m_AreaViewList[m_AreaViewList.Count - 1];
                        if (!m_PopupViewGroups.TryGetValue(topAreaViewInfo, out var popupGroup))
                        {
                            popupGroup = new PopupGroup(this);
                            m_PopupViewGroups.Add(topAreaViewInfo, popupGroup);
                        }
                        popupGroup.AddPopupView(mediator as PopupViewMediator, openUIFormInfo);
                    }
                }
            }

        }

        private void OnLoadUIFormSuccess(GameObject asset, OpenUIFormInfo openUIFormInfo)
        {
            InternalOpenUIForm(asset, openUIFormInfo);
            m_UIFormsBeingLoaded.Remove(openUIFormInfo.SerialId);
            ReferencePool.Release(openUIFormInfo);
        }

        public void WillRemovePopupView(PopupViewMediator mediator, PopupGroup popupGroup, bool isNone)
        {
            if (isNone)
            {
                if (popupGroup != m_GlobalGroup)
                {
                    popupGroup.Dispose();
                    AreaViewMediator key = null;
                    foreach (var item in m_PopupViewGroups)
                    {
                        if (item.Value == popupGroup)
                        {
                            key = item.Key;
                            break;
                        }
                    }
                    if (key != null)
                    {
                        m_PopupViewGroups.Remove(key);
                    }
                }
            }
        }

        private void OnLoadUIFormFailure(OpenUIFormInfo openUIFormInfo)
        {
            m_UIFormsBeingLoaded.Remove(openUIFormInfo.SerialId);
            ReferencePool.Release(openUIFormInfo);
        }


        public void CloseUIForm(BaseViewMediator mediator)
        {
            CloseUIForm(mediator.SerialId);
        }

        public void CloseUIForm(int serialId)
        {
            if (m_Views.TryGetValue(serialId, out BaseViewMediator mediator))
            {

                mediator.OnClose();
                GameObject.Destroy(mediator.gameObject);
                m_Views.Remove(serialId);
                var topView = m_AreaViewList.Find((mediator) => { return mediator.SerialId == serialId; });
                if (topView != null)
                {
                    m_AreaViewList.Remove(topView);
                    if (m_PopupViewGroups.TryGetValue(mediator as AreaViewMediator, out PopupGroup popupGroup))
                    {
                        popupGroup.Dispose();
                        m_PopupViewGroups.Remove(mediator as AreaViewMediator);
                    }
                }
                else
                {
                    PopupGroup popupGroup = null;
                    foreach (var item in m_PopupViewGroups)
                    {
                        if (item.Value.HasPopup(serialId))
                        {
                            popupGroup = item.Value;
                            break;
                        }
                    }
                    if (popupGroup != null)
                    {
                        popupGroup.RemovePopupView(mediator as PopupViewMediator);
                    }
                    else
                    {
                        if (m_GlobalGroup.HasPopup(serialId))
                        {
                            m_GlobalGroup.RemovePopupView(mediator as PopupViewMediator);
                        }
                    }
                }
            }
        }
    }
}