using System;
using Character;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WhoIsCatchingNaps
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelSettings _levelSettings;
        [SerializeField] private Timer _timer;
        [SerializeField] private ScoreController _scoreController;
        [SerializeField] private EndUI _endUI;
        [SerializeField] private CharacterBehaviour[] _characters;
        [Header("點擊 abnormal 時的效果")]
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private ComboView _comboView;

        private bool _isEnd;
        private bool _isRollCallActive;

        /// <summary>供技能等取得所有角色。</summary>
        public CharacterBehaviour[] GetCharacters() => _characters;
        public Action endEvent;

        /// <summary>點名技能用：設為 true 時，點到沒舉手的貓才扣時間。</summary>
        public void SetRollCallActive(bool active) => _isRollCallActive = active;

        private void Awake()
        {
            if (_timer != null)
                _timer.Initialize(_levelSettings);
            _endUI?.Hide();
            _scoreView?.Initialize(_levelSettings);
            _comboView?.Initialize(_levelSettings);
            if (SFXPlayer.instance != null)
                SFXPlayer.instance.PlayOneShot(AudioName.classBell);
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

        /// <summary>角色被點擊時：轉發計分、點錯才扣時間。點名期間以 IsHandsUp 判定。</summary>
        private void OnCharacterClicked(CharacterBehaviour character, bool isAbnormal)
        {
            if (_isRollCallActive)
            {
                bool isCorrect = character.IsHandsUp;
                _scoreController.NotifySlotClicked(character.SlotIndex, isCorrect);
                if (!isCorrect)
                    _timer.Reduce();
                return;
            }
            _scoreController.NotifySlotClicked(character.SlotIndex, isAbnormal);
            if (isAbnormal)
            {
                _scoreView?.Play().Forget();
                _comboView?.Play();
            }
            else
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
                _endUI.Show(_scoreController.Score, _scoreController.MaxCombo);
                _isEnd = true;
                Debug.Log("Level End");
            }
        }
    }
}