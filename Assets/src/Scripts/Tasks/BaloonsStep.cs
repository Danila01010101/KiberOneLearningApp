using System;
using System.Collections.Generic;

namespace KiberOneLearningApp
{
    public class BaloonsStep : ITaskStep
    {
        private List<DisableThisObjectOnClick> baloons;

        public static Action StepCompleted;

        public BaloonsStep(List<DisableThisObjectOnClick> baloons)
        {
            this.baloons = baloons;
        }

        public void Update()
        {
            bool noBaloonsLeft = true;
            
            foreach (var baloon in baloons)
            {
                if (baloon.gameObject.activeSelf)
                {
                    noBaloonsLeft = false;
                }
            }

            if (noBaloonsLeft)
            {
                StepCompleted?.Invoke();
            }
        }

        public void ResetStep()
        {
            foreach (var baloon in baloons)
            {
                if (baloon.gameObject.activeSelf == false)
                {
                    baloon.gameObject.SetActive(true);
                }
            }
        }
    }
}
