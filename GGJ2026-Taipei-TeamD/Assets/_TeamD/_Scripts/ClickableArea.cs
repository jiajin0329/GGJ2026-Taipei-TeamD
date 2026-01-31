using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ClickableArea : MonoBehaviour, IPointerDownHandler
{

// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /* 使用情境:
    - 應該要判定說目前人物狀態是怎樣，如果人物狀態是 "偷懶"，就判定為 True
    - 這個判定為 True 之後要觸發一個 event 送出去，讓其他部分可以抓到「他被抓到了」觸發其他動畫或效果之類
    - 抓到 true 之後計分要增加
    - 其實我覺得 ClickableArea 就應該直接 sent 一個 event 出去就好了？ 
        就是 物件被點擊 → 呼叫 OnPointerDown → 把這個被點到的物件 send 給 subscribers → subscribers 做反應
        --問題：1. 只要被點到就要呼叫 OnPointerDown() 嗎？ YES，一定會。但這樣就一定要 trigger event (把物件send出去) 嗎？
    */

    // 宣告 event，input 為 ClickableArea，名稱為 Clicked
    public event Action<ClickableArea> Clicked;

    // event-raising method
    public void OnPointerDown(PointerEventData eventData)
    {
        // 實作 method 
        Debug.Log("HIHI.");  // test: Log 資訊到 console
         
        // 只要按下就觸發
        Clicked?.Invoke(this);  // 傳出這個 ClickableArea instance
    }
}