namespace FreeTeam.BP.Views
{
    public interface IWeaponShotEffectEntityView : IEntityView
    {
        IParticleSystemEntityView[] ParticleSystemViews { get; }
    }
}
