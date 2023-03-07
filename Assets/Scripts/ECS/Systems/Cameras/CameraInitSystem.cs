using FreeTeam.BP.ECS.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace FreeTeam.BP.ECS.Systems
{
    public class CameraInitSystem : IEcsInitSystem
    {
        #region Inject
        private readonly EcsFilterInject<Inc<VirtualCameraData, VirtualCameraRef>> cameraFilter = default;
        private readonly EcsFilterInject<Inc<PlayerControl, TransformRef>> playerFilter = default;

        private readonly EcsPoolInject<VirtualCameraData> virtualCameraDataPool = default;
        private readonly EcsPoolInject<VirtualCameraRef> virtualCameraRefPool = default;
        private readonly EcsPoolInject<PlayerControl> playerDataPool = default;
        private readonly EcsPoolInject<TransformRef> transformRefPool = default;
        #endregion

        #region Implementation
        public void Init(IEcsSystems systems)
        {
            foreach (var camEntity in cameraFilter.Value)
            {
                ref var virtualCameraData = ref virtualCameraDataPool.Value.Get(camEntity);
                ref var virtualCameraRef = ref virtualCameraRefPool.Value.Get(camEntity);

                foreach (var playerEntity in playerFilter.Value)
                {
                    ref var playerData = ref playerDataPool.Value.Get(playerEntity);
                    ref var transformRef = ref transformRefPool.Value.Get(playerEntity);

                    virtualCameraData.targetEntity = playerData.Entity;

                    virtualCameraRef.VirtualCamera.Follow = transformRef.Transform;
                    virtualCameraRef.VirtualCamera.LookAt = transformRef.Transform;
                }
            }
        }
        #endregion
    }
}
