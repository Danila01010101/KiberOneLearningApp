using UnityEngine;
using UnityEngine.UI;

public class EmptyWindow : UIWindow
{
    [SerializeField] private UIWindow nextWindow;
    [SerializeField] private Button nextWindowButton;
    
    private bool IsButtonExist => nextWindowButton != null;
    
    public override void Initialize()
    {
        if (IsButtonExist)
            nextWindowButton.onClick.AddListener(OpenNextWindow);
        //else
            //Debug.LogError("No Window Button Exist");
    }

    private void OpenNextWindow() => UIWindowManager.Show(nextWindow, true);
}