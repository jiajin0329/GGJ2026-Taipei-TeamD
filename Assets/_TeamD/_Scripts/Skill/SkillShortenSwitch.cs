using UnityEngine;
using UnityEngine.UI;
using Character;

namespace WhoIsCatchingNaps
{
    /// <summary>技能：縮短所有貓咪狀態切換時間。掛在技能按鈕上，按鈕 On Click 綁定 TryActivate；冷卻圖示填 Cooldown Image。</summary>
    public class SkillShortenSwitch : MonoBehaviour
    {
        [Header("參考")]
        [SerializeField] private LevelManager levelManager;
        [SerializeField] [Tooltip("技能圖示 Image，Filled Radial 360，冷卻時 Fill Amount 0→1")]
        private Image cooldownImage;

        [Header("技能參數")]
        [SerializeField] private float cooldownDuration = 15f;
        [SerializeField] private float effectDuration = 5f;
        [SerializeField] [Tooltip("效果期間的狀態切換間隔（秒）")]
        private float shortenedInterval = 0.5f;

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

            foreach (var c in characters)
            {
                if (c != null)
                    c.SetSwitchInterval(shortenedInterval);
            }

            _effectRemaining = effectDuration;
            _cooldownRemaining = cooldownDuration;
            if (cooldownImage != null)
                cooldownImage.fillAmount = 0f;
        }

        private void Update()
        {
            if (_effectRemaining > 0f)
            {
                _effectRemaining -= Time.deltaTime;
                if (_effectRemaining <= 0f)
                    RestoreAll();
            }

            if (_cooldownRemaining > 0f)
                _cooldownRemaining -= Time.deltaTime;

            if (cooldownImage != null)
                cooldownImage.fillAmount = _cooldownRemaining > 0f
                    ? 1f - (_cooldownRemaining / cooldownDuration)
                    : 1f;
        }

        private void RestoreAll()
        {
            if (levelManager == null) return;

            var characters = levelManager.GetCharacters();
            if (characters == null) return;

            foreach (var c in characters)
            {
                if (c != null)
                    c.ResetSwitchInterval();
            }
        }
    }
}
