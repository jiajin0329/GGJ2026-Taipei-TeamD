using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WhoIsCatchingNaps
{
    public class EndUI : MonoBehaviour
    {
        [SerializeField] private GameObject _ui;

        [SerializeField]
        private TextMeshProUGUI _scoreText;

        [SerializeField]
        private TextMeshProUGUI _comboText;

        [SerializeField]
        private Button _restartButton;

        [SerializeField]
        private Button _exitButton;

        private void Awake()
        {
            _restartButton.onClick.AddListener(Restart);
            _exitButton.onClick.AddListener(ReturnToTitle);
            _ui.SetActive(false);
        }

        private void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        private void ReturnToTitle() => SceneManager.LoadScene(0);

        /// <summary>由 LevelManager 傳入分數與最大 Combo 顯示。</summary>
        public void Show(int score, int maxCombo)
        {
            _ui.SetActive(true);
            _scoreText.text = $"Score : {score}";
            _comboText.text = $"Combo : {maxCombo}";
            SFXPlayer.instance.PlayOneShot(AudioName.classBell);
        }

        public void Hide() => _ui.SetActive(false);
    }
}