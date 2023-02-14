namespace Generic
{
    public interface IBootable
    {
        void Init();
        void PostInit();
    }

    public interface IUpdatable
    {
        void Refresh();
        void FixedRefresh();
        void LateRefresh();
    }

    public interface IManageable : IBootable, IUpdatable
    {
    }
}