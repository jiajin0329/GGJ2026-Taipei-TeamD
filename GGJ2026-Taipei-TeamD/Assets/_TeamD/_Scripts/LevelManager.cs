using System;
using UnityEngine;

namespace WhoIsCatchingNaps
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        private LevelSettings _levelSettings;

        [SerializeField]
        private TimerView _view;

        [SerializeField]
        private Timer _timer;

        [SerializeField]
        private EndUI _endUI;

        private bool _isEnd;

        public Action endEvent;

        private void Awake()
        {
            _timer.Initialize(_levelSettings);
            _endUI.Hide();
        }

        private void Update()
        {
            LevelHandle();
        }

        private void LevelHandle()
        {
            if (_isEnd)
                return;

            _timer.Tick();
            

            if (_timer.Get() <= 0f)
            {
                endEvent?.Invoke();

                _endUI.Show();
                _isEnd = true;

                Debug.Log("Level End");
            }

            _view.SetTimerText((int)Mathf.Ceil(_timer.Get()));
        }
    }
}