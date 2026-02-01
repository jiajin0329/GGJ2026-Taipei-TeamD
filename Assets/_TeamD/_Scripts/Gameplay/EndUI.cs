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
        }

        private void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        private void ReturnToTitle() => SceneManager.LoadScene(0);

        /// <summary>由 LevelManager 傳入分數與 Combo 顯示。</summary>
        public void Show(int score, int combo)
        {
            _ui.SetActive(true);
            _scoreText.text = $"Score : {score}";
            _comboText.text = $"Combo : {combo}";
            SFXPlayer.instance.PlayOneShot(AudioName.gameEnd);
        }

        public void Hide() => _ui.SetActive(false);
    }
}