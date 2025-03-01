using System;
using UnityEngine;

namespace KiberOneLearningApp
{
    public class EmptyTask : MonoBehaviour, ITask
    {
        public bool IsCompleted { get; private set; }
        public GameObject GameObject => gameObject;

        public event Action OnTaskComplete;

        public void Setup()
        {
            IsCompleted = true;
        }
    }
}
