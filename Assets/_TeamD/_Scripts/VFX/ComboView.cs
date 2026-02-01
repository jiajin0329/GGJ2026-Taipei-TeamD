using System;
using DG.Tweening;
using UnityEngine;

namespace WhoIsCatchingNaps
{
    [Serializable]
    public class ComboView
    {
        [SerializeField]
        private Transform _comboUI;

        private LevelSettings _levelSettings;

        public void Initialize(LevelSettings _levelSettings)
        {
            this._levelSettings = _levelSettings;
        }

        public void Play()
        {
            float _scale = _levelSettings.comboDOPunchScale;

            var _sequence = DOTween.Sequence();
            _sequence.Append(_comboUI.DOScale(new Vector3(_scale, _scale, 0f), _levelSettings.doPunchScaleInDuration));
            _sequence.Append(_comboUI.DOScale(Vector3.one, _levelSettings.doPunchScaleOutDuration));
        }
    }
}