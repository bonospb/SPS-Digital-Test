using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.Views;
using Leopotam.EcsLite;
using UnityEngine;

namespace FreeTeam.BP.Behaviours.Converters
{
    public class VirtualCameraViewConverter : MonoBehaviour, IViewConverter
    {
        #region SerializeFields
        [SerializeField] private VirtualCameraView view = null;
        #endregion

        #region Implementation
        public IEntityView EntityView => view;

        public void Convert(EcsWorld world, int entity)
        {
            var virtualCameraDataPool = world.GetPool<VirtualCameraData>();
            virtualCameraDataPool.Add(entity);

            var virtualCameraRefPool = world.GetPool<VirtualCameraRef>();
            ref var virtualCameraRef = ref virtualCameraRefPool.Add(entity);
            virtualCameraRef.VirtualCamera = view.GetVirtualCamera();
        }
        #endregion
    }
}
