namespace UnityGameFramework.Runtime
{
    public enum UILayer
    {
        VIEW,
        POPUP,
        GUIDE,
        TIPS,
        SYSTEM
    }
    public enum EffectType
    {
        Bigger,
        Smaller,
        Liner
    }
    public class UIViewInfo
    {
        public string Name;
        public UILayer Layer;
        public string AssetPath;
        public string Audio;
        public bool OpenWithEffect;
        public EffectType OpenEffect;
        public bool CloseWithEffect;
        public EffectType CloseEffect;

    }

    public class AreaViewInfo
    {
        public string Name;
        public PopupGroup PopupGroup;
        public int ZOrder;
    }
}