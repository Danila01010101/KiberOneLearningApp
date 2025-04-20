using System;
using UnityEngine;

namespace KiberOneLearningApp
{
    public class GlobalValueSetter : MonoBehaviour
    {
        public static GlobalValueSetter Instance { get; private set; }

        public bool IsTeacher { get; private set; }
        
        private void SetTeacherSettings() => IsTeacher = true;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }            
            else
            {
                Destroy(this);
            }
        }
    }
}
