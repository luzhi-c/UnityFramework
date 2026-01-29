namespace GameFramework.RedDOt
{
    public abstract class IRedDot
    {
        public abstract void SetRedDotCalcHandler(GameFrameworkFunc<bool> handler);
        public abstract void AddChild(int id);
        public abstract void CalcRedDot();
        public abstract bool IsActive();
    }
}