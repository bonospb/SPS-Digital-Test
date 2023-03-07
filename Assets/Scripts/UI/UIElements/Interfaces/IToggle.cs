namespace FreeTeam.BP.UI
{ 
    public interface IToggle
    {
        bool Toggled { get; }

        void SetToggled(bool _value);

        void SetToggledWithoutNotify(bool value);
    }
}
