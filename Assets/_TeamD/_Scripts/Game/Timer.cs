using UnityEngine;

namespace WhoIsCatchingNaps
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        private TimerView _view;

        [SerializeField]
        public float _timer;

        public void Initialize(LevelSettings _levelSettings)
        {
            _timer = _levelSettings.levelTime;
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;
            _view.SetTimerText((int)Mathf.Ceil(_timer));
        }

        public float Get() => _timer;
    }
}