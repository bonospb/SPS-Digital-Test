using DG.Tweening;
using FreeTeam.BP.Editor;
using Leopotam.EcsLite;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace FreeTeam.BP.UI.Screens
{
    public class HUDScreenController : ScreenController
    {
        #region SerializeFields
        [Foldout("Labels", true)]
        [SerializeField] private TextMeshProUGUI scoreLabel = null;
        #endregion

        #region Public
        public uint Score
        {
            get => score;
            set
            {
                if (score == value)
                    return;

                score = value;
            }
        }

        public event Action OnPauseBtnClick = null;
        #endregion

        #region Private
        private EcsWorld world = default;
        private uint score = 0;
        #endregion

        #region Public methods
        //[Inject]
        public void Construct(EcsWorld ecsWorld)
        {
            world = ecsWorld;
        }

        public void Init()
        {
            RefreshUI();
        }

        public void OnPauseBtnClickHandler() =>
            OnPauseBtnClick?.Invoke();
        #endregion

        #region Private methods
        private void RefreshUI()
        {
            scoreLabel.text = $"{score}";
        }
        #endregion

        #region Coroutines
        protected override IEnumerator ShowingAnimation()
        {
            var tween = CanvasGroup.DOFade(1f, 0.5f);
            yield return tween.WaitForCompletion();
        }

        protected override IEnumerator HidingAnimation()
        {
            var tween = CanvasGroup.DOFade(0f, 0.5f);
            yield return tween.WaitForCompletion();
        }
        #endregion
    }
}
