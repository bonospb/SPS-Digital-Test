using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.Views;
using Leopotam.EcsLite;
using UnityEngine;

namespace FreeTeam.BP.Behaviours.Converters
{
    public class ArmPlaceViewConverter : MonoBehaviour, IViewConverter
    {
        #region SerializeFields
        [SerializeField] private ArmPlaceView view = null;
        #endregion

        #region Implementation
        public IEntityView EntityView => view;

        public void Convert(EcsWorld world, int entity)
        {
            var armPlaceDataPool = world.GetPool<ArmPlaceData>();
            armPlaceDataPool.Add(entity);

            var transformRefPool = world.GetPool<TransformRef>();
            ref var transformRef = ref transformRefPool.Add(entity);
            transformRef.Transform = view.GetTransform();
        }
        #endregion
    }
}
