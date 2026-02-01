using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace WhoIsCatchingNaps
{
    [Serializable]
    public class TimerView
    {
        [SerializeField]
        private TextMeshProUGUI _timerText;

        [SerializeField]
        private TextMeshProUGUI _reduceTimeText;

        private LevelSettings _levelSettings;
        private Color _startColor;

        public void Initialize(LevelSettings _levelSettings)
        {
            this._levelSettings = _levelSettings;
            _startColor = _timerText.color;
            _reduceTimeText.gameObject.SetActive(false);
            SetTimerText((int)_levelSettings.levelTime);
        }

        public void SetTimerText(int _timer)
        {
            int _min = _timer / 60;
            int _sec = _timer - (_min * 60);

            _timerText.text = $"{_min}:{_sec.ToString("D2")}" ;
        }

        public async UniTaskVoid Play()
        {
            VFX.TextEffect(_reduceTimeText, $"-{_levelSettings.reduceTime:F0}", _levelSettings.reduceTimeTextMoveY).Forget();

            _timerText.DOColor(_levelSettings.reduceTimeColor, 0.25f);
            
            float _shakeDuration = 0.25f;
            _timerText.transform.DOShakePosition(_shakeDuration, _levelSettings.reduceTimeShakeStrength, _levelSettings.reduceTimeShakeVibrato);
            await UniTask.Delay((int)(_shakeDuration*1000f));

            _timerText.DOColor(_startColor, 0.25f);
        }
    }
}