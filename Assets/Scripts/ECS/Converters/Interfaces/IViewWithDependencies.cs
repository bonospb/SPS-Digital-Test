using FreeTeam.BP.Views;
using Leopotam.EcsLite;
using System.Collections.Generic;

namespace FreeTeam.BP.Behaviours.Converters
{
    public interface IViewWithDependencies
    {
        IEntityView EntityView { get; }

        void SetEntityDependencies(EcsWorld world, int entity, IEnumerable<EcsPackedEntity> entities);
    }
}
