using UnityEngine;
using UnityEngine.InputSystem;

namespace WhoIsCatchingNaps
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        private TimerView _view;

        [SerializeField]
        public float _timer;

        [SerializeField]
        private InputActionAsset _inputActionAsset;

        [SerializeField]
        private bool _test;

        private LevelSettings _levelSettings;
        private InputAction _inputAction;

        public void Initialize(LevelSettings _levelSettings)
        {
            _timer = _levelSettings.levelTime;

            this._levelSettings = _levelSettings;

            if (_test)
            {
                _inputAction = _inputActionAsset.FindAction("Attack");
                _inputAction.Enable();
                _inputAction.started += ReduceTime;
            }

            _view.Initialize(_levelSettings);
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;
            _view.SetTimerText((int)Mathf.Ceil(_timer));
        }

        private void ReduceTime(InputAction.CallbackContext _callbackContext) => Reduce();

        public float Get() => _timer;

        public void Reduce()
        {
            _timer -= _levelSettings.reduceTime;
            _view.Reduce(_levelSettings.reduceTime).Forget();
        }

        private void OnDestroy()
        {
            if (_test)
                _inputAction.started -= ReduceTime;
        }
    }
}