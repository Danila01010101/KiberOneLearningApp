using System;
using System.Collections.Generic;
using UnityEngine;

namespace KiberOneLearningApp
{
    public class BasicTask : MonoBehaviour, ITask
    {
        [SerializeField] private List<TaskObject> objects = new List<TaskObject>();

        private int currentStep = 0;
        public bool IsCompleted { get; private set; }

        public GameObject GameObject => gameObject;

        public event Action OnTaskComplete;

        private void Update()
        {
            if (IsCompleted)
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
                IsCompleted = true;
                OnTaskComplete?.Invoke();
            }
        }

        public void Setup()
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
            [SerializeField] public ObjectForTask GameObject;
            [SerializeField] public KeyCode KeyCode = KeyCode.Mouse0;
        }
    }
}
