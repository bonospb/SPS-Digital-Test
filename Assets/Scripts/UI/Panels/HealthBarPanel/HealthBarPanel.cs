using FreeTeam.BP.Editor;
using UnityEngine;

namespace FreeTeam.BP.UI.Panels
{
    public class HealthBarPanel : PanelController
    {
        #region SerializeFields
        [Foldout("Elements")]
        [SerializeField] private ProgressBar progressBar = null;
        #endregion

        #region Public
        public float Value
        {
            get => progressBar.Value;
            set => progressBar.Value = value;
        }

        public float MinValue
        {
            get => progressBar.MinValue;
            set => progressBar.MinValue = value;
        }

        public float MaxValue
        {
            get => progressBar.MaxValue;
            set => progressBar.MaxValue = value;
        }
        #endregion
    }
}
