using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WhoIsCatchingNaps
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TimerView _view;
        [SerializeField] private InputActionAsset _inputActionAsset;

        [SerializeField] public float _timer;

        private LevelSettings _levelSettings;
        private InputAction _inputAction;

        /// <summary>按下 Attack 時觸發；扣時間僅在點擊到正常狀態時由 LevelManager 處理。</summary>
        public event Action OnAttackInput;

        public void Initialize(LevelSettings _levelSettings)
        {
            _timer = _levelSettings.levelTime;
            this._levelSettings = _levelSettings;
            _view.Initialize(_levelSettings);

            if (_inputActionAsset != null)
            {
                _inputAction = _inputActionAsset.FindAction("Attack");
                if (_inputAction != null)
                {
                    _inputAction.Enable();
                    _inputAction.started += OnAttackInputStarted;
                }
            }
        }

        public void Tick()
        {
            _timer -= Time.deltaTime;
            _view.SetTimerText((int)Mathf.Ceil(_timer));
        }

        private void OnAttackInputStarted(InputAction.CallbackContext _) => OnAttackInput?.Invoke();

        public float Get() => _timer;

        public void Reduce()
        {
            _timer -= _levelSettings.reduceTime;
            _view.Play().Forget();
        }

        private void OnDestroy()
        {
            if (_inputAction != null)
                _inputAction.started -= OnAttackInputStarted;
        }
    }
}