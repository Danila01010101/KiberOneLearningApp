using System.Collections.Generic;
using UnityEngine;

namespace KiberOneLearningApp
{
    public class MouseTask : MonoBehaviour, ITask
    {
        [SerializeField] private List<DestroyThisObjectOnClick> baloonsForLeftButton = new List<DestroyThisObjectOnClick>();
        [SerializeField] private List<DestroyThisObjectOnClick> baloonsForRightButton = new List<DestroyThisObjectOnClick>();
        
        public void OpenNextStep()
        {
            throw new System.NotImplementedException();
        }

        public void OpenPreviousStep()
        {
            throw new System.NotImplementedException();
        }
    }
}
