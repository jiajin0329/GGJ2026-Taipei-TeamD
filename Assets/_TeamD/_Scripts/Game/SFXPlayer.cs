using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WhoIsCatchingNaps
{
    [Serializable]
    public class SFXPlayer : MonoBehaviour
    {
        public static SFXPlayer instance { get; private set; }

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
            DontDestroyOnLoad(gameObject);
            instance = this;

            foreach(var audioClipSetting in _levelSettings.audioClipSettings)
            {
                _dictionaryAudioClipSetting.Add(audioClipSetting.name.ToString(), audioClipSetting);
            }

            Test();
        }

        public void PlayOneShot(AudioName _name)
        {
            var _key = _name.ToString();
            
            if (!_dictionaryAudioClipSetting.ContainsKey(_key))
                return;

            AudioClipSetting _audioClipSetting = _dictionaryAudioClipSetting[_key];

            _audioSource.PlayOneShot(_audioClipSetting.audioClip, _audioClipSetting.volume);
        }

        private void OnDestroy()
        {
            instance = null;

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