using UnityEngine;
using UnityEngine.UI;

public class EmptyWindow : UIWindow
{
    [SerializeField] private UIWindow nextWindow;
    [SerializeField] private Button nextWindowButton;
    
    public override void Initialize()
    {
        nextWindowButton.onClick.AddListener(OpenNextWindow);
    }

    private void OpenNextWindow() => UIWindowManager.Show(nextWindow, true);
}