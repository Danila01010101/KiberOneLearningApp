using System;
using System.Collections.Generic;
using UnityEngine;

namespace KiberOneLearningApp
{
    public class BaloonsStep : IStep
    {
        private List<GameObject> baloons;
        
        public Action StepCompleted { get; set; }

        public BaloonsStep(List<GameObject> baloons)
        {
            this.baloons = baloons;
        }

        public void Update()
        {
            bool noBaloonsLeft = true;
            
            foreach (var baloon in baloons)
            {
                if (baloon != null)
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
            
        }
    }
}
