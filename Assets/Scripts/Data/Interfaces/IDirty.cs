namespace FreeTeam.BP.Common
{
    public interface IDirty
    {
        bool IsDirty { get; }

        void SetDirtyWithoutNotify(bool value);
    }
}
