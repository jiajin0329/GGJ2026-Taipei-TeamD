using UnityEngine;
using UnityEngine.InputSystem;




public class ClickParticle : MonoBehaviour
{
    // 拉 Camera 進去，如果忘記拉的話就抓主camera
    [SerializeField] private Camera _targetCamera;
    // 主camera的z = -10，這裡為了要做座標轉換，要把z調整回0
    [SerializeField] private float _depth = 10f;
    [SerializeField] private ParticleSystem _clickParticle;
    [SerializeField]
    private InputActionAsset _inputActionAsset;
    private InputAction _inputAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _clickParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        
        // 初始化 camera
        if(_targetCamera == null)
        {
            _targetCamera = Camera.main;
        }

        // 訂閱 click event
        _inputAction = _inputActionAsset.FindAction("Attack");
        _inputAction.Enable();
        _inputAction.started += ScreenPositionToGame;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ScreenPositionToGame(InputAction.CallbackContext _callbackContext)
    {
        //功能
        if(Mouse.current == null) {return;}
        

        Vector2 screenPosition2 = Mouse.current.position.ReadValue();

        Vector3 screenPosition3 = new Vector3(
            screenPosition2.x,
            screenPosition2.y,
            _depth);

        Vector3 gamePosition = Camera.main.ScreenToWorldPoint(screenPosition3);
        
        transform.position = gamePosition;
        
        _clickParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        _clickParticle.transform.position = gamePosition;
        _clickParticle.Play();
    }


}



