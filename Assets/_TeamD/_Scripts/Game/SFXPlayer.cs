using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WhoIsCatchingNaps
{
    [Serializable]
    public class SFXPlayer : MonoBehaviour
    {
        [SerializeField]
        private LevelSettings _levelSettings;

        [SerializeField]
        private AudioSource _audioSource;

        private Dictionary<string, AudioClipSetting> _dictionaryAudioClipSetting = new();

#region Test
        [SerializeField]
        private InputActionAsset _inputActionAsset;
        

        [SerializeField]
        private bool _test;

        private InputAction _inputAction;
# endregion

        private void Awake()
        {
            foreach(var audioClipSetting in _levelSettings.audioClipSettings)
            {
                _dictionaryAudioClipSetting.Add(audioClipSetting.name.ToString(), audioClipSetting);
            }

            Test();
        }

        public void PlayOneShot(AudioName _name)
        {
            AudioClipSetting _audioClipSetting = _dictionaryAudioClipSetting[_name.ToString()];

            _audioSource.PlayOneShot(_audioClipSetting.audioClip, _audioClipSetting.volume);
        }

        private void OnDestroy()
        {
            if (_test)
                _inputAction.started -= TestClickSFX;
        }

        private void Test()
        {
            if (_test)
            {
                _inputAction = _inputActionAsset.FindAction("Attack");
                _inputAction.Enable();
                _inputAction.started += TestClickSFX;
            }
        }

        private void TestClickSFX(InputAction.CallbackContext _callbackContext) => PlayOneShot(AudioName.click);
    }
}