using System;

namespace FreeTeam.BP.Data
{
    public interface IProgressItem
    {
        string LevelId { get; }

        string Type { get; }

        uint Result { get; }

        event Action OnChanged;
    }
}
