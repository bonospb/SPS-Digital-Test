using UnityEngine;

namespace FreeTeam.BP.Views
{
    public interface IWeaponEntityView : IEntityView
    {
        Transform GetPointTransform();

        string ConfigId { get; set; }

        IArmPlaceEntityView ArmPlaceView { get; }
    }
}