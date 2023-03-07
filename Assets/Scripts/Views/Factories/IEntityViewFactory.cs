using FreeTeam.BP.Configuration;
using Leopotam.EcsLite;
using System.Collections.Generic;
using UnityEngine;

namespace FreeTeam.BP.Views
{
    public interface IEntityViewFactory
    {
        Dictionary<IEntityView, int> InitEntity(EcsWorld world, GameObject go, IConfig config = default);
    }
}
