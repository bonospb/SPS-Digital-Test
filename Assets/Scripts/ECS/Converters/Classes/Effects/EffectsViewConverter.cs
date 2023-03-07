using FreeTeam.BP.ECS.Components;
using FreeTeam.BP.Views;
using Leopotam.EcsLite;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FreeTeam.BP.Behaviours.Converters
{
    public class EffectsViewConverter : MonoBehaviour, IViewConverter, IViewBinder
    {
        #region SerializeFields
        [SerializeField] private EffectsView view = null;
        #endregion

        #region Implementation
        public IEntityView EntityView => view;

        public void Convert(EcsWorld world, int entity)
        {
            var effectsDataPool = world.GetPool<EffectsData>();
            effectsDataPool.Add(entity);
        }

        public void Binding(EcsWorld world, Dictionary<IEntityView, int> entities)
        {
            var entity = entities[view];

            var effectsDataPool = world.GetPool<EffectsData>();
            ref var effectsData = ref effectsDataPool.Get(entity);

            effectsData.ParticleSystemEntities = entities
                .Where(x => view.ParticleSystemViews.Contains(x.Key))
                .Select(x => world.PackEntity(x.Value))
                .ToArray();

            effectsData.TrailRendererEntities = entities
                .Where(x => view.TrailRendererViews.Contains(x.Key))
                .Select(x => world.PackEntity(x.Value))
                .ToArray();
        }
        #endregion
    }
}
