using KiberOneLearningApp;
using UnityEngine;
using UnityEngine.UI;

public class TopicChooseWindow : UIWindow
{
    [SerializeField] private UIWindow nextWindow;
    [SerializeField] private Button nextWindowButton;
    [SerializeField] private Button lessonSettingsButton;
    
    private bool IsButtonExist => nextWindowButton != null;
    
    public override void Initialize()
    {
        if (IsButtonExist)
            nextWindowButton.onClick.AddListener(OpenNextWindow);
        
        lessonSettingsButton.onClick.AddListener(ShowLessonLoadWindow);
    }

    public void ShowLessonLoadWindow() => UIWindowManager.Show<LessonLoadingWindow>();

    public override void Show()
    {
        base.Show();
        lessonSettingsButton.gameObject.SetActive(GlobalValueSetter.Instance.IsTeacher);
    }

    private void OpenNextWindow() => UIWindowManager.Show(nextWindow, true);
}