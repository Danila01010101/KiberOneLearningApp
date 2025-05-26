using System;
using UnityEngine;
using UnityEngine.Serialization;

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
