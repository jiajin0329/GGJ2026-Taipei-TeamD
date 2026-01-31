using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WhoIsCatchingNaps
{
    public class EndUI : MonoBehaviour
    {
        [SerializeField]
        private ScoreController _scoreController;

        [SerializeField]
        private GameObject _ui;

        [SerializeField]
        private TextMeshProUGUI _scoreText;

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

        public void Show()
        {
            _ui.SetActive(true);
            _scoreText.text = _scoreController.Score.ToString();
        }

        public void Hide() => _ui.SetActive(false);
    }
}