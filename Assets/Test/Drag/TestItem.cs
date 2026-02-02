using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Test
{
    public class TestItem : MonoBehaviour, IPointerClickHandler
    {
        public int col;
        public int row;
        public GameObject touch;
        public bool isTouch = false;
        // Start is called before the first frame update

        public Action<int, int> OnTouch;
        public void Init(int col, int row, bool isTouch = false)
        {
            this.col = col;
            this.row = row;
            this.isTouch = isTouch;
            touch.SetActive(isTouch);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnTouch?.Invoke(col, row);
        }

        public void SetTouch()
        {
            isTouch = !isTouch;
            touch.SetActive(isTouch);
        }


    }

}
