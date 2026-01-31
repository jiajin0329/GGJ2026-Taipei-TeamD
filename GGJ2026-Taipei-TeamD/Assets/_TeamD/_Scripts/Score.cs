using System;
using UnityEngine;

namespace WhoIsCatchingNaps
{
    public class Score : MonoBehaviour
    {
        [SerializeField]
        private ScoreView _view;

        private int _score;

        public Action<int> setEvent;
        public Action<int> adjustEvent;

        public int Get() => _score;

        public void Set(int _set)
        {
            _score = _set;
            setEvent?.Invoke(_score);
            _view.SetTimerText(_score);
        }

        public void Adjust(int _adjust)
        {
            Set(_score + _adjust);
            adjustEvent?.Invoke(_adjust);
        }
    }
}