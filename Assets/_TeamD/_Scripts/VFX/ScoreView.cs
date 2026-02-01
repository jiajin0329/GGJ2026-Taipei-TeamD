using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace WhoIsCatchingNaps
{
    [Serializable]
    public class ScoreView
    {
        [SerializeField]
        private TextMeshProUGUI _scoreText;

        [SerializeField]
        private TextMeshProUGUI _addScoreText;
        
        private LevelSettings _levelSettings;
        private Transform _scoreTextTransform;
        private Color _startColor;

        public void Initialize(LevelSettings _levelSettings)
        {
            this._levelSettings = _levelSettings;
            _scoreTextTransform = _scoreText.transform;
            _startColor = _scoreText.color;
            _addScoreText.gameObject.SetActive(false);
        }

        public async UniTaskVoid Play()
        {
            //改文字找 "+1"
            VFX.TextEffect(_addScoreText, "+1", _levelSettings.reduceTimeTextMoveY);

            _scoreText.DOColor(_levelSettings.addScoreColor, 0.25f);
            
            float _scale = _levelSettings.comboDOPunchScale;
            var _sequence = DOTween.Sequence();
            _sequence.Append(_scoreTextTransform.DOScale(new Vector3(_scale, _scale, 0f), _levelSettings.doPunchScaleInDuration));
            _sequence.Append(_scoreTextTransform.DOScale(Vector3.one, _levelSettings.doPunchScaleOutDuration));
            await UniTask.Delay((int)(0.25f*1000f));

            _scoreText.DOColor(_startColor, 0.25f);
        }
    }
}