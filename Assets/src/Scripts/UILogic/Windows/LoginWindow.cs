using UnityEngine;
using TMPro;
using System;
using System.Collections;
using ModestTree;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
    public class LoginWindow : UIWindow
    {
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private TextMeshProUGUI wrongPasswordText;
        [SerializeField] private TopicChooseWindow nextWindow;
        [SerializeField] private Button loginButton;
        [SerializeField] private Button changePasswordButton;
        [SerializeField] private Button backButton;
        
        public static Action OnLogin;
        
        public override void Initialize()
        {
            wrongPasswordText.enabled = false;
            loginButton.onClick.AddListener(Login);
            backButton.onClick.AddListener(Back);
            changePasswordButton.onClick.AddListener(ChangePassword);
        }

        public override void Show()
        {
            base.Show();
            changePasswordButton.gameObject.SetActive(GlobalValueSetter.Instance.IsTeacher);
        }

        private void Back() => UIWindowManager.Show<EnterWindow>();

        public void Login()
        {
            if (passwordInputField.text == GlobalValueSetter.Instance.Password)
            {
                OnLogin?.Invoke();
                passwordInputField.text = string.Empty;
                UIWindowManager.Show(nextWindow);
            }
            else
            {
                StartCoroutine(ShowWarning());
            }
        }

        public void ChangePassword()
        {
            if (passwordInputField.text.IsEmpty() == false)
            {
                GlobalValueSetter.Instance.ChangePassword(passwordInputField.text);
                passwordInputField.text = string.Empty;
                UIWindowManager.Show(nextWindow);
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
