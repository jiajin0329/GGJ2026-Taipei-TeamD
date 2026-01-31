using System;
using UnityEngine;

/// <summary>分數與 Combo：訂閱 OnSlotClicked，點異常加分+Combo，點正常 Combo 歸零。</summary>
public class ScoreController : MonoBehaviour
{
    [Header("計分")]
    [SerializeField] private int pointsPerCorrectClick = 10;

    [Header("選填：供 UI 訂閱")]
    [SerializeField] private UnityEngine.Events.UnityEvent<int> onScoreChanged;
    [SerializeField] private UnityEngine.Events.UnityEvent<int> onComboChanged;

    private int _score;
    private int _combo;

    public int Score => _score;
    public int Combo => _combo;
    public event Action<int> ScoreChanged;
    public event Action<int> ComboChanged;

    private void Start()
    {
        _score = 0;
        _combo = 0;
        GameEvents.OnSlotClicked += HandleSlotClicked;
        NotifyUI();
    }

    private void OnDestroy() => GameEvents.OnSlotClicked -= HandleSlotClicked;

    private void HandleSlotClicked(int slotIndex, bool isAbnormal)
    {
        if (isAbnormal)
        {
            _score += pointsPerCorrectClick;
            _combo++;
            onScoreChanged?.Invoke(_score);
            ScoreChanged?.Invoke(_score);
        }
        else
            _combo = 0;

        onComboChanged?.Invoke(_combo);
        ComboChanged?.Invoke(_combo);
    }

    private void NotifyUI()
    {
        onScoreChanged?.Invoke(_score);
        onComboChanged?.Invoke(_combo);
    }
}
