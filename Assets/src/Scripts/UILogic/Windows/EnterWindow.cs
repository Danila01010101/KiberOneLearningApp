using UnityEngine;
using UnityEngine.UI;

namespace KiberOneLearningApp
{
    public class EnterWindow : UIWindow
    { 
        [SerializeField] private UIWindow nextWindow;
        [SerializeField] private UIWindow loginWindow;
        [SerializeField] private Button nextWindowButton;
        [SerializeField] private Button loginWindowButton;
    
        private bool IsButtonExist => nextWindowButton != null;
    
        public override void Initialize()
        {
            if (IsButtonExist)
            {
                nextWindowButton.onClick.AddListener(OpenNextWindow);
                loginWindowButton.onClick.AddListener(OpenLoginWindow);
            }
        }

        private void OpenNextWindow() => UIWindowManager.Show(nextWindow, true);

        private void OpenLoginWindow() => UIWindowManager.Show(loginWindow, true);
    }
}
