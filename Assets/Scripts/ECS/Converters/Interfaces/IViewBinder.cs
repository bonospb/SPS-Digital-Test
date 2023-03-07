using FreeTeam.BP.Views;
using Leopotam.EcsLite;
using System.Collections.Generic;

namespace FreeTeam.BP.Behaviours.Converters
{
    public interface IViewBinder
    {
        void Binding(EcsWorld world, Dictionary<IEntityView, int> entities);
    }
}
