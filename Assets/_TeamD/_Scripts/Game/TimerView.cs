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
        private GameObject _reduceTimeText;

        private LevelSettings _levelSettings;
        private Color _startColor;

        public void Initialize(LevelSettings _levelSettings)
        {
            this._levelSettings = _levelSettings;
            _startColor = _timerText.color;
            _reduceTimeText.SetActive(false);
        }

        public void SetTimerText(int _timer)
        {
            int _min = _timer / 60;
            int _sec = _timer - (_min * 60);

            _timerText.text = $"{_min}:{_sec.ToString("D2")}" ;
        }

        public async UniTaskVoid Reduce(float _reduce)
        {
            ReduceTimeTextEffect();

            _timerText.DOColor(_levelSettings.reduceTimeColor, 0.25f);
            
            float _shakeDuration = 0.25f;
            _timerText.transform.DOShakePosition(_shakeDuration, _levelSettings.reduceTimeShakeStrength, _levelSettings.reduceTimeShakeVibrato);
            await UniTask.Delay((int)(_shakeDuration*1000f));

            _timerText.DOColor(_startColor, 0.25f);
        }

        private void ReduceTimeTextEffect()
        {
            var _clone = UnityEngine.Object.Instantiate(_reduceTimeText, _reduceTimeText.transform.parent);
            var _cloneTransform = _clone.transform;
            _cloneTransform.DOLocalMoveY(_cloneTransform.localPosition.y + _levelSettings.reduceTimeTextMoveY, 1f).OnComplete(() => UnityEngine.Object.Destroy(_clone));
            _clone.gameObject.SetActive(true);
        }
    }
}