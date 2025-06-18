using System;
using UnityEngine;

namespace KiberOneLearningApp
{
    public class GlobalValueSetter : MonoBehaviour
    {
        public string Password { get; private set; } = "7685";
        public static GlobalValueSetter Instance { get; private set; }

        public bool IsTeacher { get; private set; }
        
        private void SetTeacherSettings() => IsTeacher = true;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Password = PlayerPrefs.GetString("Password", Password);
                DontDestroyOnLoad(gameObject);
            }            
            else
            {
                Destroy(this);
            }
        }

        public void ChangePassword(string password)
        {
            Password = password;
            PlayerPrefs.SetString("Password", password);
        }

        private void OnEnable()
        {
            LoginWindow.OnLogin += SetTeacherSettings;
        }

        private void OnDisable()
        {
            LoginWindow.OnLogin -= SetTeacherSettings;
        }
    }
}
