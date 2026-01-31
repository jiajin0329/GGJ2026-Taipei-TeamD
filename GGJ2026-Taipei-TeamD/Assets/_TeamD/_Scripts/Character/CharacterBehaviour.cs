using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>掛在 Window/Character 上：狀態切換、點擊發送事件、受擊無敵。</summary>
[RequireComponent(typeof(RectTransform))]
public class CharacterBehaviour : MonoBehaviour, IPointerClickHandler
{
    [Header("狀態切換")]
    [SerializeField] private float switchInterval = 2f;
    [SerializeField] [Tooltip("勾選：隨機；不勾：輪流")]
    private bool randomState = true;

    [Header("狀態圖示（選填）")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite abnormalSprite;
    [SerializeField] private Sprite hitSprite;

    [Header("受擊過渡")]
    [SerializeField] private float invincibilityDuration = 0.5f;

    [Header("選填：狀態改變時綁定")]
    [SerializeField] private UnityEngine.Events.UnityEvent onStateChanged;

    private Image _image;
    private CharacterState _currentState;
    private float _timer;
    private float _invincibilityTimer;
    private int _slotIndex = -1;

    public CharacterState CurrentState => _currentState;
    public int SlotIndex => _slotIndex;
    public event Action<CharacterBehaviour> Clicked;

    private void Awake()
    {
        EnsureImageAndGraphic();
        if (_slotIndex < 0)
            _slotIndex = transform.parent != null ? transform.parent.GetSiblingIndex() : transform.GetSiblingIndex();
    }

    private void Start()
    {
        _currentState = randomState
            ? (UnityEngine.Random.value > 0.5f ? CharacterState.Normal : CharacterState.Abnormal)
            : CharacterState.Normal;
        _timer = 0f;
        ApplyStateVisual();
        onStateChanged?.Invoke();
    }

    private void Update()
    {
        if (_currentState == CharacterState.Hit)
        {
            _invincibilityTimer -= Time.deltaTime;
            if (_invincibilityTimer <= 0f)
            {
                _currentState = PickNextState();
                _timer = 0f;
                ApplyStateVisual();
                onStateChanged?.Invoke();
            }
            return;
        }

        _timer += Time.deltaTime;
        if (_timer >= switchInterval)
        {
            _timer = 0f;
            _currentState = PickNextState();
            ApplyStateVisual();
            onStateChanged?.Invoke();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentState == CharacterState.Hit)
            return;

        bool isAbnormal = _currentState == CharacterState.Abnormal;
        GameEvents.RaiseSlotClicked(_slotIndex, isAbnormal);
        Clicked?.Invoke(this);

        _currentState = CharacterState.Hit;
        _invincibilityTimer = invincibilityDuration;
        ApplyStateVisual();
        onStateChanged?.Invoke();
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

    private CharacterState PickNextState()
    {
        if (randomState)
            return UnityEngine.Random.value > 0.5f ? CharacterState.Normal : CharacterState.Abnormal;
        return _currentState == CharacterState.Normal ? CharacterState.Abnormal : CharacterState.Normal;
    }

    private void ApplyStateVisual()
    {
        if (_image == null) return;
        if (_currentState == CharacterState.Normal && normalSprite != null)
            _image.sprite = normalSprite;
        else if (_currentState == CharacterState.Abnormal && abnormalSprite != null)
            _image.sprite = abnormalSprite;
        else if (_currentState == CharacterState.Hit && hitSprite != null)
            _image.sprite = hitSprite;
    }
}
