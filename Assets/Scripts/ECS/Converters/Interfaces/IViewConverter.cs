using FreeTeam.BP.Views;
using Leopotam.EcsLite;

namespace FreeTeam.BP.Behaviours.Converters
{
    public interface IViewConverter
    {
        IEntityView EntityView { get; }

        int Priority { get => 0; }

        void Convert(EcsWorld world, int entity);
    }
}
