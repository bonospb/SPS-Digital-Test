using DG.Tweening;
using FreeTeam.BP.Editor;
using System.ComponentModel;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FreeTeam.BP.UI
{
    [ExecuteInEditMode]
    public class ProgressBar : MonoBehaviour
    {
        #region Constants
        private readonly static Regex valueRegexFormated = new Regex(@"\{Value:(?<format>[A-z0-9]*)\}|\{Value\}");
        private readonly static Regex percentRegexFormated = new Regex(@"\{Percent:(?<format>[A-z0-9]*)\}|\{Percent\}");
        private readonly static Regex minValueRegexFormated = new Regex(@"\{MinValue:(?<format>[A-z0-9]*)\}|\{MinValue\}");
        private readonly static Regex maxVaueRegexFormated = new Regex(@"\{MaxValue:(?<format>[A-z0-9]*)\}|\{MaxValue\}");
        #endregion

        #region Serialize Fields
        [Foldout("UI", true)]
        [SerializeField] protected Image progressImage = null;
        [SerializeField] protected TextMeshProUGUI valueLabel = null;

        [Foldout("Values", true)]
        [SerializeField] protected float minValue = 0.0f;
        [SerializeField] protected float maxValue = 1.0f;
        [SerializeField][Min(0)] protected float percent = 0.5f;

        [Foldout("Text Format")]
        [Tooltip("Supported: {Value}, {MinValue}, {MaxValue}, {Percent:F1}")]
        [SerializeField] protected string textPattern = "{Percent:F1}";

        [Foldout("Animation", true)]
        [SerializeField] private bool enableAnimation = false;
        [SerializeField] private float animationTime = 1f;
        #endregion

        #region Private
        private Sequence sequence = null;

        private float viewPercent = 0f;
        #endregion

        #region Public
        public float Percent
        {
            get { return percent; }
            set
            {
                var v = Mathf.Max(0, value);
                if (percent == v)
                    return;

                percent = v;

                RefreshProperties();
                RefreshText();
                RefreshUI();

                Animation();
            }
        }

        public float Value
        {
            get => Mathf.LerpUnclamped(minValue, maxValue, percent);
            set
            {
                if (Value == value)
                    return;

                Percent = InverseLerpUnclamped(minValue, maxValue, value);
            }
        }

        public float MinValue
        {
            get => minValue;
            set
            {
                var v = Mathf.Min(value, maxValue);
                if (minValue == v)
                    return;

                minValue = v;

                Percent = InverseLerpUnclamped(minValue, maxValue, Value);
            }
        }

        public float MaxValue
        {
            get => maxValue;
            set
            {
                var v = Mathf.Max(value, minValue);
                if (maxValue == v)
                    return;

                maxValue = v;

                Percent = InverseLerpUnclamped(minValue, maxValue, Value);
            }
        }

        public string TextPattern
        {
            get => textPattern;
            set
            {
                if (textPattern == value)
                    return;

                textPattern = value;

                RefreshText();
                RefreshUI();
            }
        }
        #endregion

        #region Unity methods
#if UNITY_EDITOR
        private void OnValidate() { UnityEditor.EditorApplication.delayCall += _OnValidate; }
        protected virtual void _OnValidate()
        {
            RefreshProperties();
            RefreshText();
            RefreshUI();
        }
#endif
        #endregion

        #region Public methods
        public void SetPercent(float _value)
        {
            Percent = _value;
        }

        public void SetValue(float _value)
        {
            Value = _value;
        }

        public void SetMinValue(float _minValue)
        {
            MinValue = _minValue;
        }

        public void SetMaxValue(float _maxValue)
        {
            MaxValue = _maxValue;
        }

        public void SetTextPattern(string _textPattern)
        {
            TextPattern = _textPattern;
        }
        #endregion

        #region Protected methods
        protected virtual void RefreshProperties()
        {
            viewPercent = !enableAnimation ? Percent : viewPercent;
        }

        protected virtual void RefreshText()
        {
            if (!valueLabel)
                return;

            valueLabel.text = FormatText(textPattern);
        }

        protected virtual void RefreshUI()
        {
            if (!progressImage)
                return;

            progressImage.fillAmount = viewPercent;
        }

        protected virtual void Animation()
        {
            if (!enableAnimation)
                return;

            sequence?.Kill();
            sequence = DOTween.Sequence();
            sequence.Append(DOVirtual.Float(viewPercent, Percent, animationTime,
                (value) =>
                {
                    viewPercent = value;

                    RefreshText();
                    RefreshUI();
                }));
        }
        #endregion

        #region Private methods
        private float InverseLerpUnclamped(float min, float max, float value)
        {
            if (Mathf.Abs(max - min) < Mathf.Epsilon)
                return min;

            return (value - min) / (max - min);
        }

        private string FormatText(string pattern)
        {
            var formattedText = pattern;

            var viewValue = Mathf.LerpUnclamped(minValue, maxValue, viewPercent);

            formattedText = valueRegexFormated.Replace(formattedText, m => viewValue.ToString(m.Groups[1].Value));
            formattedText = percentRegexFormated.Replace(formattedText, m => viewPercent.ToString(m.Groups[1].Value));
            formattedText = minValueRegexFormated.Replace(formattedText, m => MinValue.ToString(m.Groups[1].Value));
            formattedText = maxVaueRegexFormated.Replace(formattedText, m => MaxValue.ToString(m.Groups[1].Value));

            return formattedText;
        }
        #endregion
    }
}