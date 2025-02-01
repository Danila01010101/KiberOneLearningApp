using UnityEngine;
using UnityEngine.EventSystems;

namespace KiberOneLearningApp
{
    public class DestroyThisObjectOnClick : MonoBehaviour
    {
        [SerializeField]
        private KeyCode keyCode = KeyCode.Mouse0;
        
        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject == this && Input.GetKey(keyCode))
            {
                Destroy(gameObject);
            }
        }
    }
}
