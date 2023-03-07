using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.Views;
using Leopotam.EcsLite;
using UnityEngine;

namespace FreeTeam.BP.Behaviours.Converters
{
    public class TrailRendererViewConverter : MonoBehaviour, IViewConverter
    {
        #region SerializeFields
        [SerializeField] private TrailRendererView view = null;
        #endregion

        #region Implementation
        public IEntityView EntityView => view;

        public void Convert(EcsWorld world, int entity)
        {
            var trailRendererRefPool = world.GetPool<TrailRendererRef>();
            ref var trailRendererRef = ref trailRendererRefPool.Add(entity);
            trailRendererRef.TrailRenderer = view.GetTrailRenderer();
        }
        #endregion
    }
}
