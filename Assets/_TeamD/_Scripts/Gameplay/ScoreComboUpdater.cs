using UnityEngine;
using TMPro;

namespace WhoIsCatchingNaps
{
    /// <summary>供 ScoreController 的 On Score/Combo Changed 綁定，更新 TMP 文字（參數選動態 int）。</summary>
    public class ScoreComboUpdater : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text comboText;

        public void SetScoreText(int value)
        {
            if (scoreText != null) scoreText.text = value.ToString();
        }

        public void SetComboText(int value)
        {
            if (comboText != null) comboText.text = value.ToString();
        }
    }
}
