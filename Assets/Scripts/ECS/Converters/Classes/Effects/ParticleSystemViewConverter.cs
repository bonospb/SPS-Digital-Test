using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.Views;
using Leopotam.EcsLite;
using UnityEngine;

namespace FreeTeam.BP.Behaviours.Converters
{
    public class ParticleSystemViewConverter : MonoBehaviour, IViewConverter
    {
        #region SerializeFields
        [SerializeField] private ParticleSystemView view = null;
        #endregion

        #region Implementation
        public IEntityView EntityView => view;

        public void Convert(EcsWorld world, int entity)
        {
            var particleSystemRefPool = world.GetPool<ParticleSystemRef>();
            ref var particleSystemRef = ref particleSystemRefPool.Add(entity);
            particleSystemRef.ParticleSystem = view.GetParticleSystem();
        }
        #endregion
    }
}
