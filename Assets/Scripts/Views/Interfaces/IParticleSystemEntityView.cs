using UnityEngine;

namespace FreeTeam.BP.Views
{
    public interface IParticleSystemEntityView : IEntityView
    {
        ParticleSystem GetParticleSystem();
    }
}
