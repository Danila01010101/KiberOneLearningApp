using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
    public class LoginWindow : UIWindow
    {
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private TextMeshProUGUI wrongPasswordText;
        [SerializeField] private EmptyWindow nextWindow;
        [SerializeField] private Button loginButton;
        
        public static Action OnLogin;
        
        public override void Initialize()
        {
            wrongPasswordText.enabled = false;
            loginButton.onClick.AddListener(Login);
        }

        public void Login()
        {
            if (passwordInputField.text == GameSaver.Instance.Data.teacherPassword)
            {
                OnLogin?.Invoke();
                UIWindowManager.Show(nextWindow);
            }
            else
            {
                StartCoroutine(ShowWarning());
            }
        }

        private IEnumerator ShowWarning()
        {
            wrongPasswordText.enabled = true;
            yield return new WaitForSeconds(1.5f);
            wrongPasswordText.enabled = false;
        }
    }
}
