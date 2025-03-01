using System;
using UnityEngine;

namespace KiberOneLearningApp
{
    public interface ITask
    {
        event Action OnTaskComplete;
        GameObject GameObject { get; }
        bool IsCompleted { get; }
        void Setup();
    }
}
