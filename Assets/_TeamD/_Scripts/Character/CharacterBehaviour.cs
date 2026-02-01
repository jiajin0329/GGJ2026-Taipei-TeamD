using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Character
{
    /// <summary>掛在 Window/Character 上：狀態切換、點擊發送事件、受擊無敵。底下可放多隻貓，狀態切換時隨機換成另一隻。由 LevelManager 訂閱 Clicked 轉發計分與計時。</summary>
    [RequireComponent(typeof(RectTransform))]
    public class CharacterBehaviour : MonoBehaviour, IPointerClickHandler
    {
    private const string AnimStateSmash = "Smash";
    private const string AnimStateHandsUp = "HandsUp";

    [Header("狀態切換")]
    [SerializeField] private float switchInterval = 2f;
    [SerializeField] private bool randomState = true;

    private float _currentSwitchInterval;

    [Header("動畫")]
    [SerializeField] private Animator animator;
    [SerializeField] private Animator smashAnimator;

    [Header("動畫狀態名稱（隨機選一）")]
    [Tooltip("正常狀態")]
    [SerializeField] private string[] normalAnimStateNames = { "Normal-Idle", "Normal-ChinRest" };
    [Tooltip("異常狀態")]
    [SerializeField] private string[] abnormalAnimStateNames = { "Abnormal-EyesMaskConfuse", "Abnormal-Snooze", "Abnormal-EyesMaskLookUp" };

    [Header("受擊過渡")]
    [SerializeField] private float invincibilityDuration = 0.5f;

    [Header("選填：狀態改變時")]
    [SerializeField] private UnityEvent onStateChanged;

    [Header("多貓模式（數量>1 時狀態切換換貓）")]
    [SerializeField] private Transform[] _catTransforms;
    private int _currentCatIndex;
    private Image _image;
    private CharacterState _currentState;
    private float _timer;
    private float _invincibilityTimer;
    private float _handsUpTimer;
    private bool _isHandsUp;
    private int _slotIndex = -1;

    public CharacterState CurrentState => _currentState;
    /// <summary>點名技能期間是否正在舉手（供 LevelManager 判定點錯）。</summary>
    public bool IsHandsUp => _isHandsUp;
    public int SlotIndex => _slotIndex;
    /// <summary>參數：角色、是否為異常狀態（點擊當下即鎖定，避免動畫或時序影響）。</summary>
    public event Action<CharacterBehaviour, bool> Clicked;

    private void Awake()
    {
        EnsureImageAndGraphic();
        if (_catTransforms != null && _catTransforms.Length >= 1)
        {
            for (int i = 0; i < _catTransforms.Length; i++)
                if (_catTransforms[i] != null)
                    _catTransforms[i].gameObject.SetActive(false);
            _currentCatIndex = _catTransforms.Length > 1
                ? UnityEngine.Random.Range(0, _catTransforms.Length)
                : 0;
            if (_catTransforms[_currentCatIndex] != null)
                _catTransforms[_currentCatIndex].gameObject.SetActive(true);
            RefreshAnimatorsFromActiveCat();
        }
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

    private void RefreshAnimatorsFromActiveCat()
    {
        if (_catTransforms == null || _catTransforms.Length == 0 || _currentCatIndex < 0 || _currentCatIndex >= _catTransforms.Length)
            return;
        Transform activeCat = _catTransforms[_currentCatIndex];
        if (activeCat == null)
            return;
        Animator[] animators = activeCat.GetComponentsInChildren<Animator>(true);
        animator = animators.Length > 0 ? animators[0] : null;
        if (smashAnimator != null)
            smashAnimator.gameObject.SetActive(false);
    }

    private void SwitchToRandomCat()
    {
        if (_catTransforms == null || _catTransforms.Length <= 1)
            return;
        int count = _catTransforms.Length;
        int nextIndex = UnityEngine.Random.Range(0, count);
        while (nextIndex == _currentCatIndex && count > 1)
            nextIndex = UnityEngine.Random.Range(0, count);
        if (_catTransforms[_currentCatIndex] != null)
            _catTransforms[_currentCatIndex].gameObject.SetActive(false);
        _currentCatIndex = nextIndex;
        if (_catTransforms[_currentCatIndex] != null)
            _catTransforms[_currentCatIndex].gameObject.SetActive(true);
        RefreshAnimatorsFromActiveCat();
    }

    /// <summary>供技能等暫時縮短狀態切換間隔。結束後呼叫 ResetSwitchInterval 還原。</summary>
    public void SetSwitchInterval(float value) => _currentSwitchInterval = value;

    /// <summary>還原為預設的狀態切換間隔。</summary>
    public void ResetSwitchInterval() => _currentSwitchInterval = switchInterval;

    /// <summary>點名技能用：僅對 Normal 貓呼叫，以 CrossFade 進入 HandsUp，duration 秒後再 CrossFade 回 Normal-Idle。</summary>
    public void PlayHandsUpForDuration(float duration)
    {
        if (_currentState != CharacterState.Normal) return;
        _isHandsUp = true;
        _handsUpTimer = duration;
        if (animator != null)
            animator.CrossFade(AnimStateHandsUp, 0.25f, 0, 0f);
    }

    private void Update()
    {
        if (_currentState == CharacterState.Hit)
        {
            _invincibilityTimer -= Time.deltaTime;
            if (_invincibilityTimer <= 0f)
                TransitionToNextState();
            return;
        }

        if (_handsUpTimer > 0f)
        {
            _handsUpTimer -= Time.deltaTime;
            if (_handsUpTimer <= 0f)
            {
                _isHandsUp = false;
                if (animator != null)
                    animator.CrossFade(GetRandomNormalState(), 0.25f, 0, 0f);
            }
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
        SwitchToRandomCat();
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
        if (_isHandsUp && _handsUpTimer > 0f)
        {
            animator.Play(AnimStateHandsUp, layer, 1f);
            return;
        }
        if (_currentState == CharacterState.Normal)
            animator.Play(GetRandomNormalState(), layer, 0f);
        else if (_currentState == CharacterState.Abnormal)
            animator.Play(GetRandomAbnormalState(), layer, 0f);
    }

    private string GetRandomNormalState()
    {
        if (normalAnimStateNames != null && normalAnimStateNames.Length > 0)
            return normalAnimStateNames[UnityEngine.Random.Range(0, normalAnimStateNames.Length)];
        return "Normal-Idle";
    }

    private string GetRandomAbnormalState()
    {
        if (abnormalAnimStateNames != null && abnormalAnimStateNames.Length > 0)
            return abnormalAnimStateNames[UnityEngine.Random.Range(0, abnormalAnimStateNames.Length)];
        return "Abnormal-Snooze";
    }
}
}
