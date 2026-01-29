namespace UnityGameFramework.Runtime
{
    public abstract class BaseViewMediator : Actor
    {
        private int m_SerialId;
        public int SerialId
        {
            get => m_SerialId;
            set => m_SerialId = value;
        }
        public abstract void OnInit(object data);

        public abstract void OnResume();
        public abstract void OnPause();

        public abstract void OnClose();

    }
}