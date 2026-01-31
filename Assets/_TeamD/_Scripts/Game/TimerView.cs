using System;
using TMPro;
using UnityEngine;

namespace WhoIsCatchingNaps
{
    [Serializable]
    public struct TimerView
    {
        [SerializeField]
        private TextMeshProUGUI _timerText;

        public void SetTimerText(int _timer)
        {
            int _min = _timer / 60;
            int _sec = _timer - (_min * 60);

            _timerText.text = $"{_min}:{_sec.ToString("D2")}" ;
        }
    }
}