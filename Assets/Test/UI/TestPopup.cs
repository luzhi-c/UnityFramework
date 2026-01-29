using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class TestPopup : PopupViewMediator
{
    public override void OnInit(object data)
    {
        Debug.Log(data);
        var btn = transform.Find("btnClose");
        if (btn)
        {
            btn.GetComponent<Button>().onClick.AddListener(OnTouchMaskLayer);
        }
    }
}
