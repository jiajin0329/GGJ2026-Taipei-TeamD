using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WhoIsCatchingNaps
{
    [Serializable]
    public class ViewTester : MonoBehaviour
    {
        [SerializeField]
        private LevelSettings _levelSettings;

        [SerializeField]
        private InputActionAsset _inputActionAsset;

        private InputAction _inputAction;

        [SerializeField]
        private TimerView _timerView;

        [SerializeField]
        private ComboView _comboView;

        [SerializeField]
        private ScoreView _scoreView;

        private void Awake()
        {
            _timerView.Initialize(_levelSettings);
            _comboView.Initialize(_levelSettings);
            _scoreView.Initialize(_levelSettings);

            _inputAction = _inputActionAsset.FindAction("Attack");
            _inputAction.Enable();
            _inputAction.started += Test;
        }

        public void Test(InputAction.CallbackContext _callbackContext)
        {
            _timerView.Play().Forget();
            _comboView.Play();
            _scoreView.Play().Forget();
        }

        private void OnDestroy()
        {
            _inputAction.started -= Test;
        }
    }
}