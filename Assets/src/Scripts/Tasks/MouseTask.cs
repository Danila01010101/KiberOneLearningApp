using System.Collections.Generic;
using UnityEngine;

namespace KiberOneLearningApp
{
    public class MouseTask : MonoBehaviour, ITask
    {
        [SerializeField] private List<GameObject> taskGameOjects = new List<GameObject>();
        [SerializeField] private List<DisableThisObjectOnClick> baloonsForLeftButton = new List<DisableThisObjectOnClick>();
        [SerializeField] private List<DisableThisObjectOnClick> baloonsForRightButton = new List<DisableThisObjectOnClick>();
        [SerializeField] private List<DisableThisObjectOnClick> baloonsForMiddleButton = new List<DisableThisObjectOnClick>();

        private List<ITaskStep> steps;
        private int currentStep = 0;

        private void Awake()
        {
            steps = new List<ITaskStep>()
            {
                new BaloonsStep(baloonsForLeftButton),
                new BaloonsStep(baloonsForRightButton),
                new BaloonsStep(baloonsForMiddleButton),
            };

            OpenStep(currentStep);
        }

        public void OpenNextStep() => OpenStep(++currentStep);

        public void OpenPreviousStep() => OpenStep(--currentStep);

        private void OpenStep(int index)
        {
            if (index < 0 && index > steps.Count)
                return;

            if (index - 1 >= 0)
            {
                taskGameOjects[index - 1].SetActive(false);
                steps[index - 1].ResetStep();
            }

            taskGameOjects[index].SetActive(true);
            steps[index - 1].ResetStep();
        }
    }
}
