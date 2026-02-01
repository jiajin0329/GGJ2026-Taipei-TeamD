using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Character
{
    /// <summary>掛在 Window/Character 上：狀態切換、點擊發送事件、受擊無敵。由 LevelManager 訂閱 Clicked 轉發計分與計時。</summary>
    [RequireComponent(typeof(RectTransform))]
    public class CharacterBehaviour : MonoBehaviour, IPointerClickHandler
{
    private const string AnimStateNormalIdle = "Normal-Idle";
    private const string AnimStateAbnormalSnooze = "Abnormal-Snooze";
    private const string AnimStateSmash = "Smash";

    [Header("狀態切換")]
    [SerializeField] private float switchInterval = 2f;
    [SerializeField] private bool randomState = true;

    private float _currentSwitchInterval;

    [Header("動畫")]
    [SerializeField] private Animator animator;
    [SerializeField] private Animator smashAnimator;

    [Header("受擊過渡")]
    [SerializeField] private float invincibilityDuration = 0.5f;

    [Header("選填：狀態改變時綁定")]
    [SerializeField] private UnityEvent onStateChanged;

    private Image _image;
    private CharacterState _currentState;
    private float _timer;
    private float _invincibilityTimer;
    private int _slotIndex = -1;

    public CharacterState CurrentState => _currentState;
    public int SlotIndex => _slotIndex;
    /// <summary>參數：角色、是否為異常狀態（點擊當下即鎖定，避免動畫或時序影響）。</summary>
    public event Action<CharacterBehaviour, bool> Clicked;

    private void Awake()
    {
        EnsureImageAndGraphic();
        if (animator == null)
            animator = GetComponent<Animator>() ?? GetComponentInChildren<Animator>();
        if (smashAnimator != null)
            smashAnimator.gameObject.SetActive(false);
        EnsureSlotIndex();
    }

    private void Start()
    {
        _currentSwitchInterval = switchInterval;
        _currentState = GetInitialState();
        _timer = 0f;
        NotifyStateChange();
    }

    /// <summary>供技能等暫時縮短狀態切換間隔。結束後呼叫 ResetSwitchInterval 還原。</summary>
    public void SetSwitchInterval(float value) => _currentSwitchInterval = value;

    /// <summary>還原為預設的狀態切換間隔。</summary>
    public void ResetSwitchInterval() => _currentSwitchInterval = switchInterval;

    private void Update()
    {
        if (_currentState == CharacterState.Hit)
        {
            _invincibilityTimer -= Time.deltaTime;
            if (_invincibilityTimer <= 0f)
                TransitionToNextState();
            return;
        }

        _timer += Time.deltaTime;
        if (_timer >= _currentSwitchInterval)
            TransitionToNextState();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentState == CharacterState.Hit)
            return;

        bool isAbnormal = _currentState == CharacterState.Abnormal;
        Clicked?.Invoke(this, isAbnormal);

        _currentState = CharacterState.Hit;
        _invincibilityTimer = invincibilityDuration;
        NotifyStateChange();
    }

    public void SetSlotIndex(int index) => _slotIndex = index;

    private void EnsureImageAndGraphic()
    {
        _image = GetComponent<Image>() ?? GetComponentInChildren<Image>();
        if (GetComponent<Graphic>() != null) return;
        var transparent = gameObject.AddComponent<Image>();
        transparent.color = new Color(1f, 1f, 1f, 0f);
        transparent.raycastTarget = true;
        if (_image == null) _image = transparent;
    }

    private void EnsureSlotIndex()
    {
        if (_slotIndex >= 0) return;
        _slotIndex = transform.parent != null
            ? transform.parent.GetSiblingIndex()
            : transform.GetSiblingIndex();
    }

    private CharacterState GetInitialState()
    {
        if (randomState)
            return UnityEngine.Random.value > 0.5f ? CharacterState.Normal : CharacterState.Abnormal;
        return CharacterState.Normal;
    }

    private void TransitionToNextState()
    {
        _currentState = PickNextState();
        _timer = 0f;
        NotifyStateChange();
    }

    private void NotifyStateChange()
    {
        ApplyStateVisual();
        onStateChanged?.Invoke();
    }

    private CharacterState PickNextState()
    {
        if (randomState)
            return UnityEngine.Random.value > 0.5f ? CharacterState.Normal : CharacterState.Abnormal;
        return _currentState == CharacterState.Normal ? CharacterState.Abnormal : CharacterState.Normal;
    }

    private void ApplyStateVisual()
    {
        const int layer = 0;
        if (_currentState == CharacterState.Hit)
        {
            if (smashAnimator != null)
            {
                smashAnimator.gameObject.SetActive(true);
                smashAnimator.Play(AnimStateSmash, layer, 0f);
            }
            return;
        }

        if (smashAnimator != null)
            smashAnimator.gameObject.SetActive(false);

        if (animator == null) return;
        if (_currentState == CharacterState.Normal)
            animator.Play(AnimStateNormalIdle, layer, 0f);
        else if (_currentState == CharacterState.Abnormal)
            animator.Play(AnimStateAbnormalSnooze, layer, 0f);
    }
}
}
