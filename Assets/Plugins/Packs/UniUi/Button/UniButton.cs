using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace Worldreaver.UniUI
{
    [RequireComponent(typeof(Image))]
    public class UniButton : Button, IButton, IAffect
    {
        #region Property

#pragma warning disable 0649
        [SerializeField] private EPivot pivot = EPivot.MiddleCenter;
        [SerializeField] private bool isMotion;
        [SerializeField] private bool isCustomAudioClick;
        [SerializeField] private bool isStartGame;
        [SerializeField] private bool allowMotionDisable;
        [SerializeField] private EUIMotionType motionType = EUIMotionType.Immediate;
        [SerializeField] private EUIMotionType motionTypeDisable = EUIMotionType.Immediate;
        [SerializeField] private bool isAffectToSelf = true;
        [SerializeField] private RectTransform affectObject;
        [SerializeReference] private IMotion _motion;
        [SerializeReference] private IMotion _motionDisable;

#pragma warning restore 0649

        #endregion

        #region Implementation of IButton

        public EPivot Pivot => pivot;
        public bool IsMotion { get => isMotion; set => isMotion = value; }
        public bool AllowMotionDisable => allowMotionDisable;
        public bool IsRelease { get; private set; }
        public bool IsPrevent { get; private set; }

        public EUIMotionType MotionType { get => motionType; private set => motionType = value; }

        public EUIMotionType MotionTypeDisable { get => motionTypeDisable; private set => motionTypeDisable = value; }

        public IMotion Motion => _motion;
        public IMotion MotionDisable => _motionDisable;

        #endregion

        #region Implementation of IAffect

        public Vector3 DefaultScale { get; set; }
        public bool IsAffectToSelf => isAffectToSelf;

        public RectTransform AffectObject => IsAffectToSelf ? targetGraphic.rectTransform : affectObject;

        #endregion

        #region Overrides of UIBehaviour

        protected override void Start()
        {
            base.Start();
            InitializeMotion();
            
            if (!IsMotion) return;
            DefaultScale = AffectObject.localScale;
        }

        #region Overrides of Selectable

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            //use Invoke to call method to avoid warning "Send Message cannot be called during Awake, or OnValidate...

            Invoke(nameof(InitializeMotion), 0.1f);
        }
        
#endif
        public override void OnPointerExit(
            PointerEventData eventData)
        {
            if (IsRelease) return;
            base.OnPointerExit(eventData);
            IsPrevent = true;
            OnPointerUp(eventData);
        }

        #endregion

        #region Overrides of Button

        public override void OnPointerDown(
            PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            IsRelease = false;
            IsPrevent = false;
            if (!IsMotion) return;
            if (!interactable && AllowMotionDisable)
            {
                _motionDisable?.MotionDown(DefaultScale, AffectObject);
                return;
            }

            _motion?.MotionDown(DefaultScale, AffectObject);
        }

        public override void OnPointerUp(
            PointerEventData eventData)
        {
            if (IsRelease) return;
            base.OnPointerUp(eventData);
            IsRelease = true;
            if (!IsMotion) return;
            if (!interactable && AllowMotionDisable)
            {
                _motionDisable?.MotionUp(DefaultScale, AffectObject);
                return;
            }

            _motion?.MotionUp(DefaultScale, AffectObject);
        }

        public override void OnPointerClick(
            PointerEventData eventData)
        {
            if (IsRelease && IsPrevent) return;
            base.OnPointerClick(eventData);
        }

        #endregion

        protected void InitializeMotion()
        {
            if (!IsMotion) return;

            if (_motion == null)
            {
                switch (MotionType)
                {
                    case EUIMotionType.Immediate:
                        _motion = new MotionImmediate();
                        break;
                    case EUIMotionType.NormalCurve:
                        _motion = new MotionCurve();
                        break;
                    case EUIMotionType.NormalEase:
                        _motion = new MotionEase();
                        break;
                    case EUIMotionType.UniformCurve:
                        _motion = new UniformMotionCurve();
                        break;
                    case EUIMotionType.UniformEase:
                        _motion = new UniformMotionEase();
                        break;
                    case EUIMotionType.LateCurve:
                        _motion = new LateMotionCurve();
                        break;
                    case EUIMotionType.LateEase:
                        _motion = new LateMotionEase();
                        break;
                }
            }
            else
            {
                switch (MotionType)
                {
                    case EUIMotionType.Immediate when _motion.GetType() != typeof(MotionImmediate):
                        _motion = new MotionImmediate();
                        break;
                    case EUIMotionType.NormalCurve when _motion.GetType() != typeof(MotionCurve):
                        _motion = new MotionCurve();
                        break;
                    case EUIMotionType.NormalEase when _motion.GetType() != typeof(MotionEase):
                        _motion = new MotionEase();
                        break;
                    case EUIMotionType.UniformCurve when _motion.GetType() != typeof(UniformMotionCurve):
                        _motion = new UniformMotionCurve();
                        break;
                    case EUIMotionType.UniformEase when _motion.GetType() != typeof(UniformMotionEase):
                        _motion = new UniformMotionEase();
                        break;
                    case EUIMotionType.LateCurve when _motion.GetType() != typeof(LateMotionCurve):
                        _motion = new LateMotionCurve();
                        break;
                    case EUIMotionType.LateEase when _motion.GetType() != typeof(LateMotionEase):
                        _motion = new LateMotionEase();
                        break;
                }
            }

            if (!AllowMotionDisable) return;

            if (_motionDisable == null)
            {
                switch (MotionTypeDisable)
                {
                    case EUIMotionType.Immediate:
                        _motionDisable = new MotionImmediate();
                        break;
                    case EUIMotionType.NormalCurve:
                        _motionDisable = new MotionCurve();
                        break;
                    case EUIMotionType.NormalEase:
                        _motionDisable = new MotionEase();
                        break;
                    case EUIMotionType.UniformCurve:
                        _motionDisable = new UniformMotionCurve();
                        break;
                    case EUIMotionType.UniformEase:
                        _motionDisable = new UniformMotionEase();
                        break;
                    case EUIMotionType.LateCurve:
                        _motionDisable = new LateMotionCurve();
                        break;
                    case EUIMotionType.LateEase:
                        _motionDisable = new LateMotionEase();
                        break;
                }
            }
            else
            {
                switch (MotionTypeDisable)
                {
                    case EUIMotionType.Immediate when _motionDisable.GetType() != typeof(MotionImmediate):
                        _motionDisable = new MotionImmediate();
                        break;
                    case EUIMotionType.NormalCurve when _motionDisable.GetType() != typeof(MotionCurve):
                        _motionDisable = new MotionCurve();
                        break;
                    case EUIMotionType.NormalEase when _motionDisable.GetType() != typeof(MotionEase):
                        _motionDisable = new MotionEase();
                        break;
                    case EUIMotionType.UniformCurve when _motionDisable.GetType() != typeof(UniformMotionCurve):
                        _motionDisable = new UniformMotionCurve();
                        break;
                    case EUIMotionType.UniformEase when _motionDisable.GetType() != typeof(UniformMotionEase):
                        _motionDisable = new UniformMotionEase();
                        break;
                    case EUIMotionType.LateCurve when _motionDisable.GetType() != typeof(LateMotionCurve):
                        _motionDisable = new LateMotionCurve();
                        break;
                    case EUIMotionType.LateEase when _motionDisable.GetType() != typeof(LateMotionEase):
                        _motionDisable = new LateMotionEase();
                        break;
                }
            }
        }

        #endregion
    }
}