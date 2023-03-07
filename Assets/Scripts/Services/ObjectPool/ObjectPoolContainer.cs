namespace FreeTeam.BP.Services.ObjectPool
{
    public class ObjectPoolContainer<T>
    {
        #region Public
        public bool Used { get; private set; }

        public T Item { get; set; }
        #endregion

        #region Public methods
        public void Consume() =>
            Used = true;

        public void Release() =>
            Used = false;
        #endregion
    }
}