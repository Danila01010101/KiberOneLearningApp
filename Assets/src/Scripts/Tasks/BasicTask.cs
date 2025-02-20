using System;
using System.Collections.Generic;
using UnityEngine;

namespace KiberOneLearningApp
{
    public class BasicTask : MonoBehaviour
    {
        [SerializeField] private List<TaskObject> objects = new List<TaskObject>();

        private int currentStep = 0;
        private bool isTaskRunning = true;
        
        public Action OnTaskComplete;

        private void Update()
        {
            if (isTaskRunning == false)
                return;
            
            bool noOjectsLeft = true;
            
            foreach (var disablableGameobject in objects)
            {
                if (disablableGameobject.GameObject.gameObject.activeSelf)
                {
                    noOjectsLeft = false;
                }
            }

            if (noOjectsLeft)
            {
                isTaskRunning = false;
                OnTaskComplete?.Invoke();
            }
        }

        public void ResetStep()
        {
            foreach (var taskObject in objects)
            {
                taskObject.GameObject.gameObject.SetActive(true);
                taskObject.GameObject.SetKeyCode(taskObject.KeyCode);
            }
        }

        [Serializable]
        public class TaskObject
        {
            [SerializeField] public readonly DisableThisObjectOnClick GameObject;
            [SerializeField] public readonly KeyCode KeyCode = KeyCode.Mouse0;
        }
    }
}
