namespace Leopotam.EcsLite
{
    public static class EcsWorldExtensions
    {
        public static ref T AddUnique<T>(this EcsWorld world) where T : struct =>
            ref world.GetPool<T>().Add(0);

        public static ref T GetUnique<T>(this EcsWorld world) where T : struct =>
            ref world.GetPool<T>().Get(0);

        public static void DelUnique<T>(this EcsWorld world) where T : struct =>
            world.GetPool<T>().Del(0);

        public static bool HasUnique<T>(this EcsWorld world) where T : struct =>
            world.GetPool<T>().Has(0);

        public static ref T ReplaceUnique<T>(this EcsWorld world) where T : struct
        {
            if (world.HasUnique<T>())
                world.DelUnique<T>();

            return ref world.AddUnique<T>();
        }
    }

    public static class EcsFilterExtensions
    {
        public static int GetFirstEntity(this EcsFilter filter)
        {
            var enumerator = filter.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current;
        }

        public static bool IsEmpty(this EcsFilter ecsFilter) =>
            ecsFilter.GetEntitiesCount() <= 0;
    }

    public static class EcsPoolExtensions
    {
        public static ref T Replace<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (pool.Has(entity))
                pool.Del(entity);

            return ref pool.Add(entity);
        }

        public static ref T GetOrDefault<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (pool.Has(entity))
                return ref pool.Get(entity);

            return ref pool.Add(entity);
        }
    }
}
