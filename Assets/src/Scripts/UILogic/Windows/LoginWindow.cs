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
        [SerializeField] private TopicChooseWindow nextWindow;
        [SerializeField] private Button loginButton;
        [SerializeField] private Button backButton;
        
        public static Action OnLogin;
        
        public override void Initialize()
        {
            wrongPasswordText.enabled = false;
            loginButton.onClick.AddListener(Login);
            backButton.onClick.AddListener(Back);
        }

        private void Back() => UIWindowManager.Show<EnterWindow>();

        public void Login()
        {
            if (passwordInputField.text == GlobalValueSetter.Instance.Password)
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
