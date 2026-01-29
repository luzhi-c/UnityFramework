namespace UnityGameFramework.Runtime
{
    public class PopupViewMediator : BaseViewMediator
    {
        public static bool CloseOnTouchMaskLayer = true;
        public override void OnInit(object data)
        {

        }

        public override void OnResume()
        {

        }
        public override void OnPause()
        {

        }

        public override void OnClose()
        {

        }

        public void OnTouchMaskLayer()
        {
            if (CloseOnTouchMaskLayer)
            {
                UIManager.Instance.CloseUIForm(this);
            }
        }

    }
}