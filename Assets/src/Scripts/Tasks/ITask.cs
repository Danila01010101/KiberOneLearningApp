using System;
using UnityEngine;

namespace KiberOneLearningApp
{
    public interface ITask
    {
        Action OnTaskComplete { get; set; }
        GameObject GameObject { get; }
        bool IsCompleted { get; }
        void Setup();
    }
}
