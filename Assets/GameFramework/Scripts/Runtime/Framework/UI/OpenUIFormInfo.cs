using GameFramework;

namespace UnityGameFramework.Runtime
{
    public class OpenUIFormInfo : IReference
    {
        private int m_SerialId;
        private string m_UIFormName;
        private object m_UserData;
        private bool m_Bind;

        public static OpenUIFormInfo Create(int serialId, string uiFormName, object userData, bool bind)
        {
            var data = ReferencePool.Acquire<OpenUIFormInfo>();
            data.m_SerialId = serialId;
            data.m_UIFormName = uiFormName;
            data.m_UserData = userData;
            data.m_Bind = bind;
            return data;
        }
        public void Clear()
        {
            m_SerialId = 0;
            m_UIFormName = null;
            m_UserData = null;
            m_Bind = false;
        }

        public int SerialId
        {
            get
            {
                return m_SerialId;
            }
        }

        public string UIFormName
        {
            get
            {
                return m_UIFormName;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }
        public bool Bind
        {
            get
            {
                return m_Bind;
            }
        }
    }
}