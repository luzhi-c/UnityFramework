using GameFramework;

namespace UnityGameFramework.Runtime
{
    public class LoadAssetCallbacks<T>
    {
        private GameFrameworkAction<T> m_LoadAssetSuccessCallback;
        private GameFrameworkAction m_LoadAssetFailureCallback;
        private GameFrameworkAction<float> m_LoadAssetUpdateCallback;


        /// <summary>
        /// 获取加载资源成功回调函数。
        /// </summary>
        public GameFrameworkAction<T> LoadAssetSuccessCallback
        {
            get
            {
                return m_LoadAssetSuccessCallback;
            }
        }

        /// <summary>
        /// 获取加载资源失败回调函数。
        /// </summary>
        public GameFrameworkAction LoadAssetFailureCallback
        {
            get
            {
                return m_LoadAssetFailureCallback;
            }
        }
        /// <summary>
        /// 获取加载资源更新回调函数。
        /// </summary>
        public GameFrameworkAction<float> LoadAssetUpdateCallback
        {
            get
            {
                return m_LoadAssetUpdateCallback;
            }
        }
    }
}