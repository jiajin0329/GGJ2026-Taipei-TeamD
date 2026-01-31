using System;
using TMPro;
using UnityEngine;

namespace WhoIsCatchingNaps
{
    [Serializable]
    public struct ScoreView
    {
        [SerializeField]
        private TextMeshProUGUI[] _scoreTexts;

        public void SetTimerText(int _score)
        {
            foreach(var _scoreText in _scoreTexts)
            {
                _scoreText.text = _score.ToString();
            }
        }
    }
}