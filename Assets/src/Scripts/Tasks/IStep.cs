using System;

namespace KiberOneLearningApp
{
    public interface IStep
    {
        Action StepCompleted { get; set; }
        void Update();
        void ResetStep();
    }
}
