using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Test
{
    public class Test_Drag : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
    {
        public GameObject target;
        private bool isTouch;

        public void OnPointerDown(PointerEventData eventData)
        {
            isTouch = true;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (!isTouch)
            {
                return;
            }
            target.transform.position = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isTouch = false;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
