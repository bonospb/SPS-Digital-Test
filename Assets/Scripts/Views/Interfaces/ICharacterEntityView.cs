using UnityEngine;

namespace FreeTeam.BP.Views
{
    public interface ICharacterEntityView : IEntityView
    {
        string ConfigId { get; set; }

        IArmPlaceEntityView[] ArmPlaceViews { get; }
    }
}
