using System;
using TMPro;
using UnityEngine;

namespace WhoIsCatchingNaps
{
    [Serializable]
    public struct LevelManagerView
    {
        [SerializeField]
        private TextMeshProUGUI _timerText;

        public void SetTimerText(int _timer)
        {
            _timerText.text = _timer.ToString();
        }
    }
}