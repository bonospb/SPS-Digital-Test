namespace FreeTeam.BP.Views
{
    public interface IEffectsEntityView : IEntityView
    {
        IParticleSystemEntityView[] ParticleSystemViews { get; }
        ITrailRendererEntityView[] TrailRendererViews { get; }
    }
}
