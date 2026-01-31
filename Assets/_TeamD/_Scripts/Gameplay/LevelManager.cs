using System;
using UnityEngine;
using Character;

namespace WhoIsCatchingNaps
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelSettings _levelSettings;
        [SerializeField] private Timer _timer;
        [SerializeField] private ScoreController _scoreController;
        [SerializeField] private EndUI _endUI;
        [SerializeField] private CharacterBehaviour[] _characters;

        private bool _isEnd;
        public Action endEvent;

        private void Awake()
        {
            _timer.Initialize(_levelSettings);
            _endUI.Hide();
        }

        private void Start()
        {
            if (_characters != null)
            {
                foreach (var c in _characters)
                {
                    if (c != null)
                        c.Clicked += OnCharacterClicked;
                }
            }
        }

        private void OnDestroy()
        {
            if (_characters != null)
            {
                foreach (var c in _characters)
                {
                    if (c != null)
                        c.Clicked -= OnCharacterClicked;
                }
            }
        }

        /// <summary>角色被點擊時：轉發計分、點錯才扣時間。isAbnormal 由 Character 在點擊當下傳入，避免動畫或時序造成誤判。</summary>
        private void OnCharacterClicked(CharacterBehaviour character, bool isAbnormal)
        {
            _scoreController.NotifySlotClicked(character.SlotIndex, isAbnormal);
            if (!isAbnormal)
                _timer.Reduce();
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
                _endUI.Show(_scoreController.Score, _scoreController.Combo);
                _isEnd = true;
                Debug.Log("Level End");
            }
        }
    }
}