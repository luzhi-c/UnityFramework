
namespace GameFramework.RedDOt
{
    public class RedDotArgs : GameFrameworkEventArgs
    {
        public int ID;

        public static RedDotArgs Create(int id)
        {
            var msg = ReferencePool.Acquire<RedDotArgs>();
            msg.ID = id;
            return msg;
        }

        public override void Clear()
        {
            ID = 0;
        }
    }
}