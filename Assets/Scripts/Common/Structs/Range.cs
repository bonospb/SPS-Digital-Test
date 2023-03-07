using System;

[Serializable]
public struct Range
{
    #region Public
    public float Min;
    public float Max;
    #endregion

    #region Public methods
    public bool Contains(float value) =>
        (value >= Min && value < Max);

    public override string ToString() =>
        $"Min: {Min}, Max: {Max}";
    #endregion
}