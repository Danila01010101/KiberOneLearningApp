using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KiberOneLearningApp
{
    public class DisableThisObjectOnClick : MonoBehaviour
    {
        private KeyCode keyCode = KeyCode.Mouse0;

        public void SetKeyCode(KeyCode keyCode) => this.keyCode = keyCode;
        
        private void Update()
        {
            if (Input.GetKey(keyCode))
            {
                PointerEventData pointer = new PointerEventData(EventSystem.current);
                pointer.position = Input.mousePosition;

                List<RaycastResult> raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointer, raycastResults);

                if (raycastResults.Count > 0)
                {
                    foreach (var go in raycastResults)
                    {
                        if (go.gameObject.name == gameObject.name)
                        gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
