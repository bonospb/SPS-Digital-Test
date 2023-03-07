using FreeTeam.BP.Behaviours.Converters;
using FreeTeam.BP.Configuration;
using Leopotam.EcsLite;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FreeTeam.BP.Views
{
    public class EntityViewFactory : IEntityViewFactory
    {
        #region Implementations
        public Dictionary<IEntityView, int> InitEntity(EcsWorld world, GameObject prefab, IConfig config = default)
        {
            var allGO = prefab.GetComponentsInChildren<Transform>().Select(x => x.gameObject);
            var entities = new Dictionary<IEntityView, int>();

            foreach (var go in allGO)
            {
                var viewConverters = go.GetComponents<IViewConverter>();

                if (viewConverters.Length <= 0)
                    continue;

                var entity = world.NewEntity();

                foreach (var viewConverter in viewConverters)
                {
                    Create(world, entity, viewConverter, config);

                    entities.Add(viewConverter.EntityView, entity);
                }
            }

            foreach (var go in allGO)
            {
                var viewBinders = go.GetComponents<IViewBinder>();
                foreach (var viewBinder in viewBinders)
                {
                    Binding(world, viewBinder, entities);
                }

                var viewsWithDependencies = go.GetComponents<IViewWithDependencies>();
                foreach (var withDependencies in viewsWithDependencies)
                {
                    withDependencies.SetEntityDependencies(
                        world,
                        entities[withDependencies.EntityView],
                        entities.Values.Select(x => world.PackEntity(x)));
                }
            }

            return entities;
        }
        #endregion

        #region Private methods
        private void Create(EcsWorld world, int entity, IViewConverter viewConverter, IConfig config = default)
        {
            if (viewConverter.EntityView is IWithConfigId viewWithConfig)
                viewWithConfig.ConfigId = config.Id;

            viewConverter.Convert(world, entity);
        }

        private void Binding(EcsWorld world, IViewBinder viewBinder, Dictionary<IEntityView, int> entities) =>
            viewBinder.Binding(world, entities);
        #endregion
    }
}
