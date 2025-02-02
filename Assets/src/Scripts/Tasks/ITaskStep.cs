using System;

namespace KiberOneLearningApp
{
    public interface ITaskStep
    {
        static Action StepCompleted { get; set; }
        void Update();
        void ResetStep();
    }
}
