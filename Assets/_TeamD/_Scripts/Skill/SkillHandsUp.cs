using UnityEngine;
using UnityEngine.UI;
using Character;
using WhoIsCatchingNaps;

namespace WhoIsCatchingNaps
{
    /// <summary>技能「點名」：只有 Normal 貓播放 HandsUp 舉手 3 秒，點名期間沒舉手＝點錯。掛在另一顆技能按鈕上，冷卻 10 秒。</summary>
    public class SkillHandsUp : MonoBehaviour
    {
        [Header("參考")]
        [SerializeField] private LevelManager levelManager;
        [SerializeField] [Tooltip("技能圖示 Image，Filled Radial 360，冷卻時 Fill Amount 0→1")]
        private Image cooldownImage;

        [Header("技能參數")]
        [SerializeField] private float cooldownDuration = 10f;
        [SerializeField] private float effectDuration = 3f;

        private float _cooldownRemaining;
        private float _effectRemaining;

        private void Awake()
        {
            if (cooldownImage == null)
                cooldownImage = GetComponent<Image>();
        }

        private void Start()
        {
            if (cooldownImage != null)
                cooldownImage.fillAmount = 1f;
        }

        /// <summary>供按鈕 On Click 綁定。</summary>
        public void TryActivate()
        {
            if (_cooldownRemaining > 0f)
                return;

            if (levelManager == null)
                return;

            var characters = levelManager.GetCharacters();
            if (characters == null)
                return;

            levelManager.SetRollCallActive(true);

            foreach (var c in characters)
            {
                if (c != null && c.CurrentState == CharacterState.Normal)
                    c.PlayHandsUpForDuration(effectDuration);
            }

            _effectRemaining = effectDuration;
            _cooldownRemaining = cooldownDuration;
            if (cooldownImage != null)
                cooldownImage.fillAmount = 0f;

            SFXPlayer.instance?.PlayOneShot(AudioName.skill_rollCall);
        }

        private void Update()
        {
            if (_effectRemaining > 0f)
            {
                _effectRemaining -= Time.deltaTime;
                if (_effectRemaining <= 0f)
                    levelManager.SetRollCallActive(false);
            }

            if (_cooldownRemaining > 0f)
                _cooldownRemaining -= Time.deltaTime;

            if (cooldownImage != null)
                cooldownImage.fillAmount = _cooldownRemaining > 0f
                    ? 1f - (_cooldownRemaining / cooldownDuration)
                    : 1f;
        }
    }
}
