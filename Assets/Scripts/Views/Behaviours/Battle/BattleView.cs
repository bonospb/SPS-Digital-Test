using UnityEngine;

namespace FreeTeam.BP.Views
{
    public class BattleView : MonoBehaviour, IBattleEntityView, IWithConfigId
    {
        #region SerializeFields
        [SerializeField] private int group = 1;
        #endregion

        #region Implementation
        public Transform GetTransform() => transform;

        public string ConfigId { get; set; }

        public int Group => group;
        #endregion
    }
}
