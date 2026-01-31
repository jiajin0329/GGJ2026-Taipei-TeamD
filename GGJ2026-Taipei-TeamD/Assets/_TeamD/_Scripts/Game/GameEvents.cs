using System;

/// <summary>Character 點擊事件，供 ScoreController／計時等訂閱。</summary>
public static class GameEvents
{
    public static event Action<int, bool> OnSlotClicked;
    public static void RaiseSlotClicked(int slotIndex, bool isAbnormal) => OnSlotClicked?.Invoke(slotIndex, isAbnormal);
}
