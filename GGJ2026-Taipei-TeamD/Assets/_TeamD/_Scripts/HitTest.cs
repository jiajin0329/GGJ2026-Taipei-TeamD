using UnityEngine;

public class HitTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 這是一個測試腳本，測試去訂閱 ClickableArea.Clicked 的事件能不能得到正常反應
    [SerializeField] private ClickableArea targetArea;

    private void OnEnable()
    {
        if (targetArea != null)
        {
            targetArea.Clicked += OnAreaClicked;
        }
    }

    private void OnDisable()
    {
        if (targetArea != null)
        {
            targetArea.Clicked -= OnAreaClicked;
        }
    }

    private void OnAreaClicked(ClickableArea area)
    {
        Debug.Log($"Clicked area: {area.name}");
    }


}
