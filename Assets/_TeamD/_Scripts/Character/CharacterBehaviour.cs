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
    private const string HitImageChildName = "HitImage";

    [Header("狀態切換")]
    [SerializeField] private float switchInterval = 2f;
    [SerializeField] private bool randomState = true;

    [Header("狀態圖示")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite abnormalSprite;
    [SerializeField]
    private GameObject hitImage;

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
    public event Action<CharacterBehaviour> Clicked;

    private void Awake()
    {
        EnsureImageAndGraphic();
        EnsureHitImageResolved();
        EnsureSlotIndex();
    }

    private void Start()
    {
        _currentState = GetInitialState();
        _timer = 0f;
        NotifyStateChange();
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

        _timer += Time.deltaTime;
        if (_timer >= switchInterval)
            TransitionToNextState();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_currentState == CharacterState.Hit)
            return;

        Clicked?.Invoke(this);

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

    private void EnsureHitImageResolved()
    {
        if (hitImage == null)
        {
            var t = transform.Find(HitImageChildName);
            if (t != null) hitImage = t.gameObject;
        }
        if (hitImage != null)
            hitImage.SetActive(false);
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
        if (hitImage != null)
            hitImage.SetActive(_currentState == CharacterState.Hit);

        if (_image == null || _currentState == CharacterState.Hit) return;
        if (_currentState == CharacterState.Normal && normalSprite != null)
            _image.sprite = normalSprite;
        else if (_currentState == CharacterState.Abnormal && abnormalSprite != null)
            _image.sprite = abnormalSprite;
    }
}
}
