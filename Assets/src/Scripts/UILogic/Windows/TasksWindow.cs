using System;
using UnityEngine;

namespace KiberOneLearningApp
{
    public class TasksWindow : UIWindow
    {
        [SerializeField] private TaskCreator taskCreator;
        
        public override void Initialize()
        {
            
        }

        private void OnEnable()
        {
            taskCreator.Subscribe();
        }

        private void OnDisable()
        {
            taskCreator.Unsubscribe();
        }
    }
}
