using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GuideMaskController : MonoBehaviour
{
    [SerializeField] private GameObject targetUI; // 需要点击的目标
    [SerializeField] private GameObject blockPanel; // 全屏屏蔽层
    public Button testBtn;
    public Button testBtn1;

    void Start()
    {
        testBtn.onClick.AddListener(OnClick);
        testBtn1.onClick.AddListener(OnClick);

        var trigger = blockPanel.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnPointerClick((PointerEventData)data); });
        trigger.triggers.Add(entry);

    }

    public void SetTarget(GameObject go)
    {
        targetUI = go;
    }

    void OnClick()
    {
        Debug.Log("点击到了");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 检查点击是否在目标UI区域内
        if (RectTransformUtility.RectangleContainsScreenPoint(
            targetUI.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera))
        {
            // 触发目标UI的点击事件
            ExecuteEvents.Execute(targetUI.gameObject, eventData,
                ExecuteEvents.pointerClickHandler);
        }
        else
        {
            eventData.Use();
        }
    }
}
