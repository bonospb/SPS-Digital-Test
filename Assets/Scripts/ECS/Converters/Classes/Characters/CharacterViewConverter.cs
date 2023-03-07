using FreeTeam.BP.Configuration;
using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.Views;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace FreeTeam.BP.Behaviours.Converters
{
    public class CharacterViewConverter : MonoBehaviour, IViewConverter
    {
        #region SerializeFileds
        [SerializeField] private CharacterView view = null;
        #endregion

        #region Private
        private Configurations configurations = null;
        #endregion

        #region Public methods
        [Inject]
        public void Construct(Configurations configs) =>
            configurations = configs;
        #endregion

        #region Implementation
        public IEntityView EntityView => view;

        public void Convert(EcsWorld world, int entity)
        {
            var config = configurations.Avatars[view.ConfigId];

            var avatarConfigRefPool = world.GetPool<ConfigRef>();
            ref var avatarConfigRef = ref avatarConfigRefPool.Add(entity);
            avatarConfigRef.Config = config;

            var transformRefPool = world.GetPool<TransformRef>();
            ref var transformRef = ref transformRefPool.Replace(entity);
            transformRef.Transform = view.GetTransform();
        }
        #endregion
    }
}
